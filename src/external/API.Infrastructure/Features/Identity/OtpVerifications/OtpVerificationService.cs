using API.Application.Features.Authentication.Services;
using API.Application.Features.Business;
using API.Application.Features.Identity.OtpVerifications;
using API.Application.Features.Identity.OtpVerifications.Commands.VerifyOtp;
using API.Domain.Common.Models.Messages;
using API.Domain.Common.Models.Results;
using API.Domain.Entities.Identity.OtpVerifications;
using API.Domain.Features.Identity.OtpVerifications;
using API.Domain.Features.Identity.OtpVerifications.Enums;
using API.Domain.Features.Identity.Users;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Security.Cryptography;

namespace API.Infrastructure.Features.Identity.OtpVerifications
{
    internal class OtpVerificationService : IOtpVerificationService
    {
        private const int MaxFailedAttempts = 5;

        private readonly IUserRepository _userRepository;
        private readonly IOtpVerificationRepository _otpVerificationRepository;

        private readonly IOtpHasher _otpHasher;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<OtpVerificationService> _logger;
        private readonly IEmailService _emailService;
        private readonly IJwtProvider _jwtProvider;

        public OtpVerificationService(
            IUserRepository userRepository,
            IOtpVerificationRepository otpVerificationRepository,
            IOtpHasher otpHasher,
            IUnitOfWork unitOfWork,
            IEmailService emailService,
            IJwtProvider jwtProvider,
            ILogger<OtpVerificationService> logger)
        {
            _userRepository = userRepository;
            _otpVerificationRepository = otpVerificationRepository;
            _otpHasher = otpHasher;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _jwtProvider = jwtProvider;
            _logger = logger;
        }

        public async Task InitializeAsync(
            User user,
            CancellationToken cancellationToken = default)
        {
            var verificationCode = GenerateCode();
            var otpVerification = new OtpVerification(
                user.Id,
                _otpHasher.HashOtp(verificationCode),
                OtpPurpose.EmailVerification);
            _otpVerificationRepository.Add(otpVerification);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Generated OTP verification code for user {Email}: {Code}", user.Email, verificationCode);

            try
            {
                var message = new EmailMessage(user.UserName, verificationCode);
                await _emailService.SendEmailAsync(user.Email, message, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Send verification email failed.");
            }
        }

        public async Task<Result> VerifyOtpAsync(
            VerifyOtpCommand command,
            CancellationToken cancellationToken = default)
        {
            var user = await _userRepository
                .FindByEmailAsync(command.Body.Email, cancellationToken);
            if (user is null)
            {
                return Result
                    .Fail(Error.NotFound("User"), HttpStatusCode.NotFound);
            }

            var otpVerification = await _otpVerificationRepository
                .FindActiveAsync(user.Id, command.Body.Purpose, cancellationToken);
            if (otpVerification is null || !otpVerification.IsAvailable || otpVerification.UsedAt.HasValue)
            {
                return Result
                    .Fail("OTP code is invalid or has already been used.", HttpStatusCode.BadRequest);
            }

            if (otpVerification.ExpiresAt < DateTime.UtcNow)
            {
                otpVerification.Invalidate();
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result
                    .Fail("OTP code has expired.", HttpStatusCode.BadRequest);
            }

            if (otpVerification.FailedAttempts >= MaxFailedAttempts)
            {
                otpVerification.Invalidate();
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result
                    .Fail("Maximum failed attempts reached. Please request a new OTP code.", HttpStatusCode.TooManyRequests);
            }

            if (!_otpHasher.VerifyOtp(command.Body.Otp, otpVerification.CodeHash))
            {
                otpVerification.IncrementFailedAttempt();
                if (otpVerification.FailedAttempts >= MaxFailedAttempts)
                {
                    otpVerification.Invalidate();
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                if (!otpVerification.IsAvailable)
                {
                    return Result
                        .Fail("Maximum failed attempts reached. Please request a new OTP code.", HttpStatusCode.TooManyRequests);
                }

                return Result
                    .Fail("Invalid OTP code.", HttpStatusCode.BadRequest);
            }

            otpVerification.MarkAsUsed();
            otpVerification.Invalidate();

            switch (command.Body.Purpose)
            {
                case OtpPurpose.EmailVerification:
                    user.CompleteVerification();
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    return Result.Succeed("Email verified successfully.");

                case OtpPurpose.PasswordReset:
                    var resetToken = _jwtProvider.GeneratePasswordResetToken(user.Id);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    return Result<string>.Succeed(resetToken, "OTP verified successfully.");

                default:
                    return Result.Fail("Unsupported OTP purpose.", HttpStatusCode.BadRequest);
            }
        }

        private string GenerateCode()
        {
            var random = RandomNumberGenerator.GetInt32(100000, 999999);
            return random.ToString();
        }
    }
}
