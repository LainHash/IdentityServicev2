using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Identity.Roles.Commands.Delete
{
    internal class DeleteRoleCommandHandler(IRoleService roleService)
        : IRequestHandler<DeleteRoleCommand, Result>
    {
        private readonly IRoleService _roleService = roleService;

        public async Task<Result> Handle(
            DeleteRoleCommand request,
            CancellationToken cancellationToken)
        {
            return await _roleService.DeleteAsync(request, cancellationToken);
        }
    }
}

