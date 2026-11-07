using API.Contract.DTOs.Identity.Roles;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Identity.Roles.Queries.GetById
{
    internal class GetRoleByIdQueryHandler(IRoleService roleService)
        : IRequestHandler<GetRoleByIdQuery, APIResponse<RoleDetailResponse>>
    {
        private readonly IRoleService _roleService = roleService;

        public async Task<APIResponse<RoleDetailResponse>> Handle(
            GetRoleByIdQuery request,
            CancellationToken cancellationToken)
        {
            var specification = new GetRoleByIdSpecification(request);
            return await _roleService.GetByIdAsync(specification, cancellationToken);
        }
    }
}

