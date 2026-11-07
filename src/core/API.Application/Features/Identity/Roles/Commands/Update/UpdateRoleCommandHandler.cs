using API.Contract.DTOs.Identity.Roles;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Identity.Roles.Commands.Update
{
    internal class UpdateRoleCommandHandler(IRoleService roleService)
        : IRequestHandler<UpdateRoleCommand, APIResponse<RoleResponse>>
    {
        private readonly IRoleService _roleService = roleService;

        public async Task<APIResponse<RoleResponse>> Handle(
            UpdateRoleCommand request,
            CancellationToken cancellationToken)
        {
            return await _roleService.UpdateAsync(request, cancellationToken);
        }
    }
}

