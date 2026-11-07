using API.Application.Common.Services;
using API.Application.Features.Authentication.Commands.GoogleLogin;
using API.Application.Features.Authentication.Commands.Login;
using API.Application.Features.Authentication.Commands.Logout;
using API.Application.Features.Authentication.Commands.RefreshToken;
using API.Application.Features.Authentication.Commands.Register;
using API.Contract.DTOs.Authentication;
using API.Contract.Settings;
using API.Domain.Common.Models.Results;
using API.Domain.Features.Identity.UserResfreshTokens;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.RegularExpressions;
using Google.Apis.Auth;
using API.Domain.Common.Models.Messages;
using API.Domain.Features.Identity.Users;
using API.Domain.Features.Identity.Roles;
using API.Application.Features.Game.Players;
using API.Application.Features.Identity.OtpVerifications;
using API.Application.Features.Authentication;
using API.Application.Features.Authentication.Services;

namespace API.Infrastructure.Features.Authentication
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRefreshTokenRepository _userRefreshTokenRepository;

        private readonly IOtpVerificationService _otpVerificationService;
        private readonly IPlayerService _playerService;

        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;
        private readonly JwtSettings _jwtSettings;
        private readonly GoogleAuthSettings _googleSettings;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IJwtProvider jwtProvider,
            IMapper mapper,
            ILogger<AuthenticationService> logger,
            IOptions<AppSettings> options,
            IUnitOfWork unitOfWork,
            IRoleRepository roleRepository,
            IUserRefreshTokenRepository userRefreshTokenRepository,
            IOtpVerificationService otpVerificationService,
            IPlayerService playerService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
            _mapper = mapper;
            _logger = logger;
            _jwtSettings = options.Value.Jwt;
            _googleSettings = options.Value.GoogleAuth;
            _unitOfWork = unitOfWork;
            _roleRepository = roleRepository;
            _userRefreshTokenRepository = userRefreshTokenRepository;
            _otpVerificationService = otpVerificationService;
            _playerService = playerService;
        }

        public async Task<APIResponse<AuthenticationResponse>> LoginAsync(
            LoginCommand command,
            CancellationToken cancellationToken = default)
        {
            var user = await _userRepository
                .FindByIdentifierAsync(command.Body.Identifier, cancellationToken);
            if (user is null || !_passwordHasher.VerifyPassword(command.Body.Password, user.PasswordHash))
            {
                return APIResponse<AuthenticationResponse>.Fail(
                    "Incorrect email, username, or password.", 
                    HttpStatusCode.Unauthorized);
            }

            if (!user.IsActive)
            {
                return APIResponse<AuthenticationResponse>.Fail(
                    "Account is not active. Please verify your email.", 
                    HttpStatusCode.PreconditionRequired);
            }

            var role = await _roleRepository
                .FindByIdAsync(user.RoleId, cancellationToken);
            var roleName = role?.Name ?? "Player";

            var accessToken = _jwtProvider
                .GenerateToken(user.PublicId, user.UserName, user.Email, roleName);

            var rawRefreshToken = _jwtProvider
                .GenerateRefreshToken();

            var tokenHash = _jwtProvider
                .HashToken(rawRefreshToken);

            var refreshToken = new UserRefreshToken(
                user.Id,
                tokenHash,
                DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays));

            _userRefreshTokenRepository.Add(refreshToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = new AuthenticationResponse(user, accessToken, rawRefreshToken);
            return APIResponse<AuthenticationResponse>
                .Succeed(response, "Login successfully.");
        }

        public async Task<APIResponse<AuthenticationResponse>> GoogleLoginAsync(
            GoogleLoginCommand command,
            CancellationToken cancellationToken = default)
        {
            GoogleJsonWebSignature.Payload payload;
            try
            {
                var validationSettings = new GoogleJsonWebSignature.ValidationSettings();
                var audiences = new List<string>();
                if (!string.IsNullOrWhiteSpace(_googleSettings?.ClientId))
                    audiences.Add(_googleSettings.ClientId);
                if (!string.IsNullOrWhiteSpace(_googleSettings?.AndroidClientId))
                    audiences.Add(_googleSettings.AndroidClientId);
                if (!string.IsNullOrWhiteSpace(_googleSettings?.IosClientId))
                    audiences.Add(_googleSettings.IosClientId);

                if (audiences.Count > 0)
                {
                    validationSettings.Audience = audiences;
                }

                payload = await GoogleJsonWebSignature
                    .ValidateAsync(command.Body.IdToken, validationSettings);
            }
            catch (InvalidJwtException ex)
            {
                _logger.LogWarning(ex, "Invalid Google IdToken received.");
                return APIResponse<AuthenticationResponse>
                    .Fail("Token Google không hợp lệ hoặc đã hết hạn.", HttpStatusCode.Unauthorized);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error verifying Google IdToken.");
                return APIResponse<AuthenticationResponse>
                    .Fail("Xác thực tài khoản Google thất bại.", HttpStatusCode.Unauthorized);
            }

            if (payload is null || string.IsNullOrWhiteSpace(payload.Email))
            {
                return APIResponse<AuthenticationResponse>
                    .Fail("Không thể lấy thông tin email từ tài khoản Google.", HttpStatusCode.BadRequest);
            }

            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var user = await _userRepository.FindByEmailAsync(payload.Email, cancellationToken);
                if (user is null)
                {
                    var playerRole = await _roleRepository.FindByNameAsync("Player", cancellationToken);
                    if (playerRole is null)
                    {
                        return APIResponse<AuthenticationResponse>
                            .Fail("Không tìm thấy vai trò khách hàng (Player).", HttpStatusCode.InternalServerError);
                    }

                    var rawPrefix = payload.Email.Split('@')[0];
                    var cleanBase = Regex.Replace(rawPrefix, @"[^a-zA-Z0-9_]", "");
                    if (string.IsNullOrWhiteSpace(cleanBase))
                    {
                        cleanBase = "user";
                    }
                    if (cleanBase.Length > 30)
                    {
                        cleanBase = cleanBase[..30];
                    }

                    var proposedUsername = cleanBase;
                    var suffix = 1;
                    while (await _userRepository.IsExistingUserNameAsync(null, proposedUsername, cancellationToken))
                    {
                        proposedUsername = $"{cleanBase}_{suffix++}";
                    }

                    var randomPasswordHash = _passwordHasher.HashPassword(Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N"));
                    user = User.Create(proposedUsername, payload.Email, randomPasswordHash, playerRole.Id, isActive: true);
                    _userRepository.Add(user);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    await _playerService.InitializeAsync(user, cancellationToken);
                }
                else
                {
                    if (!user.IsActive)
                    {
                        user.SetActive(true);
                    }
                }

                var role = await _roleRepository.FindByIdAsync(user.RoleId, cancellationToken);
                var roleName = role?.Name ?? "Player";

                var accessToken = _jwtProvider.GenerateToken(user.PublicId, user.UserName, user.Email, roleName);
                var rawRefreshToken = _jwtProvider.GenerateRefreshToken();
                var tokenHash = _jwtProvider.HashToken(rawRefreshToken);

                var refreshToken = new UserRefreshToken(
                    user.Id,
                    tokenHash,
                    DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays));

                _userRefreshTokenRepository.Add(refreshToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                var response = new AuthenticationResponse(user, accessToken, rawRefreshToken);
                return APIResponse<AuthenticationResponse>
                    .Succeed(response, "Đăng nhập Google thành công.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogError(ex, "Google login failed for email {Email}", payload.Email);
                return APIResponse<AuthenticationResponse>
                    .Fail("Đăng nhập bằng tài khoản Google không thành công.", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<APIResponse> RegisterAsync(
            RegisterCommand command,
            CancellationToken cancellationToken = default)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var existingUser = await _userRepository
                    .FindByEmailAsync(command.Body.Email, cancellationToken);
                if (existingUser is not null)
                {
                    return APIResponse.Fail(
                        "This email already used. Please use another email.", 
                        HttpStatusCode.Conflict);
                }

                var playerRole = await _roleRepository
                    .FindByNameAsync("Player", cancellationToken);
                if (playerRole is null)
                {
                    return APIResponse.Fail(
                        Error.NotFound("Role"), 
                        HttpStatusCode.InternalServerError);
                }

                var user = _mapper.Map<User>(command.Body)
                    .SetPasswordHash(_passwordHasher.HashPassword(command.Body.Password))
                    .SetRole(playerRole.Id);
                _userRepository.Add(user);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                await _playerService
                    .InitializeAsync(user, cancellationToken);

                await _otpVerificationService
                    .InitializeAsync(user, cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return APIResponse.Succeed(
                    "Register successfully. Please check your account to get verification code.", 
                    HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);

                _logger.LogError(ex, "Register request failed. Email: {Email}", command.Body.Email);
                return APIResponse.Fail(
                    "Register request failed.",
                    HttpStatusCode.InternalServerError);
            }
        }

        public async Task<APIResponse<AuthenticationResponse>> RefreshTokenAsync(
            RefreshTokenCommand command,
            CancellationToken cancellationToken = default)
        {
            var tokenHash = _jwtProvider.HashToken(command.Body.RefreshToken);
            var storedToken = await _userRefreshTokenRepository.FindActiveByTokenHashAsync(tokenHash, cancellationToken);

            if (storedToken is null || !storedToken.IsActive)
            {
                return APIResponse<AuthenticationResponse>
                    .Fail("Invalid or expired refresh token.", HttpStatusCode.Unauthorized);
            }

            var user = await _userRepository.FindByIdWithRoleAsync(storedToken.User.PublicId, cancellationToken);
            if (user is null || !user.IsActive)
            {
                return APIResponse<AuthenticationResponse>
                    .Fail("User not found or account is inactive.", HttpStatusCode.Unauthorized);
            }

            var role = await _roleRepository.FindByIdAsync(user.RoleId, cancellationToken);
            var roleName = role?.Name ?? "Player";

            storedToken.Revoke();

            var newAccessToken = _jwtProvider.GenerateToken(user.PublicId, user.UserName, user.Email, roleName);
            var newRawRefreshToken = _jwtProvider.GenerateRefreshToken();
            var newTokenHash = _jwtProvider.HashToken(newRawRefreshToken);

            var newRefreshToken = new UserRefreshToken(
                storedToken.UserId,
                newTokenHash,
                DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays));

            _userRefreshTokenRepository.Add(newRefreshToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = new AuthenticationResponse(user, newAccessToken, newRawRefreshToken);
            return APIResponse<AuthenticationResponse>
                .Succeed(response, "Token refreshed successfully.");
        }

        public async Task<APIResponse> LogoutAsync(
            LogoutCommand command,
            CancellationToken cancellationToken = default)
        {
            var tokenHash = _jwtProvider.HashToken(command.RefreshToken);
            var storedToken = await _userRefreshTokenRepository.FindActiveByTokenHashAsync(tokenHash, cancellationToken);

            if (storedToken is null)
            {
                return APIResponse.Succeed("Logged out successfully.");
            }

            storedToken.Revoke();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return APIResponse.Succeed("Logged out successfully.");
        }

        public async Task<APIResponse> LogoutAllAsync(
            LogoutAllCommand command,
            CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.FindByIdAsync(command.UserId, cancellationToken);
            if (user is null)
            {
                return APIResponse.Fail("User not found.", HttpStatusCode.NotFound);
            }

            await _userRefreshTokenRepository.RevokeAllByUserIdAsync(user.Id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return APIResponse.Succeed("All sessions logged out successfully.");
        }

    }
}
