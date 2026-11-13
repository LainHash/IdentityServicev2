using API.Application.Features.Authentication.Services;
using API.Application.Features.Business;
using API.Application.Features.Identity.Users;
using API.Application.Features.Identity.Users.Commands.ChangePassword;
using API.Application.Features.Identity.Users.Commands.Delete;
using API.Application.Features.Identity.Users.Commands.ForceChangePassword;
using API.Application.Features.Identity.Users.Commands.Restore;
using API.Application.Features.Identity.Users.Commands.ToggleActive;
using API.Application.Features.Identity.Users.Commands.ToggleLock;
using API.Application.Features.Identity.Users.Queries.GetAll;
using API.Application.Features.Identity.Users.Queries.GetById;
using API.Contract.DTOs.Identity.Users;
using API.Domain.Common.Models.Messages;
using API.Domain.Common.Models.Results;
using API.Domain.Features.Identity.UserResfreshTokens;
using API.Domain.Features.Identity.Users;
using AutoMapper;
using System.Net;

namespace API.Infrastructure.Features.Identity.Users
{
    internal class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRefreshTokenRepository _userRefreshTokenRepository;

        private readonly IPasswordHasher _passwordHasher;

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(
            IUserRepository userRepository,
            IMapper mapper,
            IPasswordHasher passwordHasher,
            IUnitOfWork unitOfWork,
            IUserRefreshTokenRepository userRefreshTokenRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
            _userRefreshTokenRepository = userRefreshTokenRepository;
        }

        public async Task<PaginationResult<IEnumerable<UserResponse>>> GetAllAsync(
            GetAllUsersSpecification specification, 
            CancellationToken cancellationToken = default)
        {
            var users = await _userRepository
                .ToListAsync(specification, cancellationToken);

            var totalItems = await _userRepository
                .CountAsync(specification, cancellationToken);

            var skip = specification.Skip;
            var take = specification.Take;

            var response = _mapper.Map<IEnumerable<UserResponse>>(users);
            return PaginationResult<IEnumerable<UserResponse>>
                .Succeed(response, Success.Retrieved("Users"), totalItems, skip, take);
        }

        public async Task<Result<UserDetailResponse>> GetByIdAsync(
            GetUserByIdSpecification specification, 
            CancellationToken cancellationToken = default)
        {
            var user = await _userRepository
                .FindAsync(specification, cancellationToken);
            if(user is null)
            {
                return Result<UserDetailResponse>
                    .Fail(Error.NotFound("User"), HttpStatusCode.NotFound);
            }

            var response = _mapper.Map<UserDetailResponse>(user);
            return Result<UserDetailResponse>
                .Succeed(response, Success.Retrieved("User"));
        }


        public async Task<Result> ChangePasswordAsync(
            ChangePasswordCommand command,
            CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.FindByIdAsync(command.UserId, cancellationToken);
            if (user is null || !user.IsActive)
            {
                return Result
                    .Fail(Error.NotFound("User"), HttpStatusCode.NotFound);
            }

            if (!_passwordHasher.VerifyPassword(command.Body.OldPassword, user.PasswordHash))
            {
                return Result
                    .Fail("Current password is incorrect.", HttpStatusCode.BadRequest);
            }

            var newPasswordHash = _passwordHasher.HashPassword(command.Body.NewPassword);
            user.SetPasswordHash(newPasswordHash);

            await _userRefreshTokenRepository.RevokeAllByUserIdAsync(user.Id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result
                .Succeed("Password changed successfully. Please login with your new password.");
        }

        public async Task<Result> DeleteAsync(
            DeleteUserCommand command, 
            CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.FindByIdAsync(command.Id, cancellationToken);
            if (user is null)
            {
                return Result
                    .Fail(Error.NotFound("User"), HttpStatusCode.NotFound);
            }

            if (!user.IsConfirmed)
            {
                _userRepository.Remove(user);
            }
            else
            {
                if (user.IsDeleted)
                {
                    return Result
                        .Fail(Error.AlreadyDeleted("User"), HttpStatusCode.Conflict);
                }

                user.SoftDelete();
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result
                .Succeed(Success.Deleted("User"));
        }

        public async Task<Result> RestoreAsync(
            RestoreUserCommand command,
            CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.FindByIdAsync(command.Id, cancellationToken);
            if (user is null)
            {
                return Result
                    .Fail(Error.NotFound("User"), HttpStatusCode.NotFound);
            }

            if(!user.IsDeleted)
            {
                return Result
                    .Fail(Error.NotYetDeleted("User"), HttpStatusCode.Conflict);
            }

            user.Restore();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result
                .Succeed(Success.Restored("User"));
        }

        public async Task<Result> ForceChangePasswordAsync(
            ForceChangePasswordCommand command, 
            CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.FindByIdAsync(command.Id, cancellationToken);
            if (user is null || !user.IsActive)
            {
                return Result
                    .Fail(Error.NotFound("User"), HttpStatusCode.NotFound);
            }

            var newPasswordHash = _passwordHasher.HashPassword(command.NewPassword);
            user.SetPasswordHash(newPasswordHash);

            await _userRefreshTokenRepository.RevokeAllByUserIdAsync(user.Id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result
                .Succeed("Password changed successfully.");
        }

        public async Task<Result> ToggleLockAsync(
            ToggleLockUserCommand command,
            CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.FindByIdAsync(command.Id, cancellationToken);
            if (user is null || user.IsDeleted)
            {
                return Result
                    .Fail(Error.NotFound("User"), HttpStatusCode.NotFound);
            }

            user.ToggleLock();

            if (user.IsLocked)
            {
                await _userRefreshTokenRepository.RevokeAllByUserIdAsync(user.Id, cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var message = user.IsLocked
                ? Success.Locked("User")
                : Success.Unlocked("User");

            return Result.Succeed(message);
        }

        public async Task<Result> ToggleActiveAsync(
            ToggleActiveUserCommand command,
            CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.FindByIdAsync(command.Id, cancellationToken);
            if (user is null || user.IsDeleted)
            {
                return Result
                    .Fail(Error.NotFound("User"), HttpStatusCode.NotFound);
            }

            user.ToggleActive();

            if (!user.IsActive)
            {
                await _userRefreshTokenRepository.RevokeAllByUserIdAsync(user.Id, cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var message = user.IsActive
                ? Success.Activated("User")
                : Success.Deactivated("User");

            return Result.Succeed(message);
        }
    }
}
