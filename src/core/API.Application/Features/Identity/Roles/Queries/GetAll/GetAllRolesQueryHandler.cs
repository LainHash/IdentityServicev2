using API.Contract.DTOs.Identity.Roles;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Identity.Roles.Queries.GetAll
{
    internal class GetAllRolesQueryHandler(IRoleService roleService)
        : IRequestHandler<GetAllRolesQuery, PaginationResult<IEnumerable<RoleResponse>>>
    {
        private readonly IRoleService _roleService = roleService;

        public async Task<PaginationResult<IEnumerable<RoleResponse>>> Handle(
            GetAllRolesQuery request,
            CancellationToken cancellationToken)
        {
            var specification = new GetAllRolesSpecification(request);
            return await _roleService.GetAllAsync(specification, cancellationToken);
        }
    }
}

