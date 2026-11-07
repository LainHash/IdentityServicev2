using API.Application.Features.Identity.Roles.Commands.Create;
using API.Application.Features.Identity.Roles.Commands.Delete;
using API.Application.Features.Identity.Roles.Commands.Restore;
using API.Application.Features.Identity.Roles.Commands.Update;
using API.Application.Features.Identity.Roles.Queries.GetAll;
using API.Application.Features.Identity.Roles.Queries.GetById;
using API.Contract.DTOs.Identity.Roles;
using API.Domain.Common.Models.Results;

namespace API.Application.Features.Identity.Roles
{
    public interface IRoleService
    {
        Task<PaginationResponse<IEnumerable<RoleResponse>>> GetAllAsync(
            GetAllRolesSpecification specification,
            CancellationToken cancellationToken = default);

        Task<APIResponse<RoleDetailResponse>> GetByIdAsync(
            GetRoleByIdSpecification specification,
            CancellationToken cancellationToken = default);

        Task<APIResponse<RoleResponse>> CreateAsync(
            CreateRoleCommand command,
            CancellationToken cancellationToken = default);

        Task<APIResponse<RoleResponse>> UpdateAsync(
            UpdateRoleCommand command,
            CancellationToken cancellationToken = default);

        Task<APIResponse> DeleteAsync(
            DeleteRoleCommand command,
            CancellationToken cancellationToken = default);

        Task<APIResponse> RestoreAsync(
            RestoreRoleCommand command,
            CancellationToken cancellationToken = default);
    }
}

