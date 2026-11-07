using API.Contract.DTOs.Identity.Roles;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Identity.Roles.Commands.Create
{
    internal class CreateRoleCommandHandler(IRoleService roleService)
        : IRequestHandler<CreateRoleCommand, Result<RoleResponse>>
    {
        private readonly IRoleService _roleService = roleService;

        public async Task<Result<RoleResponse>> Handle(
            CreateRoleCommand request,
            CancellationToken cancellationToken)
        {
            return await _roleService.CreateAsync(request, cancellationToken);
        }
    }
}

