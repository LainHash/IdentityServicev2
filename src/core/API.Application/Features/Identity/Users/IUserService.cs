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
        Task<PaginationResponse<IEnumerable<UserResponse>>> GetAllAsync(
            GetAllUsersSpecification specification,
            CancellationToken cancellationToken = default);

        Task<APIResponse<UserResponse>> GetByIdAsync(
            GetUserByIdSpecification specification,
            CancellationToken cancellationToken = default);

        Task<APIResponse> ChangePasswordAsync(
            ChangePasswordCommand command,
            CancellationToken cancellationToken = default);

        Task<APIResponse> DeleteAsync(
            DeleteUserCommand command,
            CancellationToken cancellationToken = default);

        Task<APIResponse> RestoreAsync(
            RestoreUserCommand command,
            CancellationToken cancellationToken = default);

        Task<APIResponse> ForceChangePasswordAsync(
            ForceChangePasswordCommand command,
            CancellationToken cancellationToken = default);

        Task<APIResponse> ToggleLockAsync(
            ToggleLockUserCommand command,
            CancellationToken cancellationToken = default);

        Task<APIResponse> ToggleActiveAsync(
            ToggleActiveUserCommand command,
            CancellationToken cancellationToken = default);
    }
}
