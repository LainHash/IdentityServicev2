using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Identity.Roles.Commands.Restore
{
    internal class RestoreRoleCommandHandler(IRoleService roleService)
        : IRequestHandler<RestoreRoleCommand, APIResponse>
    {
        private readonly IRoleService _roleService = roleService;

        public async Task<APIResponse> Handle(
            RestoreRoleCommand request,
            CancellationToken cancellationToken)
        {
            return await _roleService.RestoreAsync(request, cancellationToken);
        }
    }
}

