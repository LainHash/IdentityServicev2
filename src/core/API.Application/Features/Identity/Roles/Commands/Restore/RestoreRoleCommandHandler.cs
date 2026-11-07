using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Identity.Roles.Commands.Restore
{
    internal class RestoreRoleCommandHandler(IRoleService roleService)
        : IRequestHandler<RestoreRoleCommand, Result>
    {
        private readonly IRoleService _roleService = roleService;

        public async Task<Result> Handle(
            RestoreRoleCommand request,
            CancellationToken cancellationToken)
        {
            return await _roleService.RestoreAsync(request, cancellationToken);
        }
    }
}

