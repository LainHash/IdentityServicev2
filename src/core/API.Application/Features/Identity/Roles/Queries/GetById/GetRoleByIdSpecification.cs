using API.Domain.Common.Specifications;
using API.Domain.Features.Identity.Roles;

namespace API.Application.Features.Identity.Roles.Queries.GetById
{
    public class GetRoleByIdSpecification
        : BaseSpecification<Role>
    {
        public GetRoleByIdSpecification(GetRoleByIdQuery query)
        {
            AddInclude(x => x.Users);

            AddCriteria(x => x.PublicId == query.Id);
        }
    }
}

