using API.Application.Features.Identity.Users.Commands.ChangePassword;
using API.Application.Features.Identity.Users.Commands.Delete;
using API.Application.Features.Identity.Users.Commands.ForceChangePassword;
using API.Application.Features.Identity.Users.Commands.Restore;
using API.Application.Features.Identity.Users.Commands.ToggleActive;
using API.Application.Features.Identity.Users.Commands.ToggleLock;
using API.Application.Features.Identity.Users.Queries.GetAll;
using API.Application.Features.Identity.Users.Queries.GetById;
using API.Contract.DTOs.Identity.Users;
using API.Domain.Common.Models.Results;

namespace API.Application.Features.Identity.Users
{
    public interface IUserService
    {
        Task<PaginationResult<IEnumerable<UserResponse>>> GetAllAsync(
            GetAllUsersSpecification specification,
            CancellationToken cancellationToken = default);

        Task<Result<UserResponse>> GetByIdAsync(
            GetUserByIdSpecification specification,
            CancellationToken cancellationToken = default);

        Task<Result> ChangePasswordAsync(
            ChangePasswordCommand command,
            CancellationToken cancellationToken = default);

        Task<Result> DeleteAsync(
            DeleteUserCommand command,
            CancellationToken cancellationToken = default);

        Task<Result> RestoreAsync(
            RestoreUserCommand command,
            CancellationToken cancellationToken = default);

        Task<Result> ForceChangePasswordAsync(
            ForceChangePasswordCommand command,
            CancellationToken cancellationToken = default);

        Task<Result> ToggleLockAsync(
            ToggleLockUserCommand command,
            CancellationToken cancellationToken = default);

        Task<Result> ToggleActiveAsync(
            ToggleActiveUserCommand command,
            CancellationToken cancellationToken = default);
    }
}
