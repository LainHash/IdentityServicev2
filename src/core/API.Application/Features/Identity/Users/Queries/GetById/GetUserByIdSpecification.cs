using API.Domain.Common.Specifications;
using API.Domain.Features.Identity.Users;

namespace API.Application.Features.Identity.Users.Queries.GetById
{
    public class GetUserByIdSpecification
        : BaseSpecification<User>
    {
        public GetUserByIdSpecification(GetUserByIdQuery query)
        {
            AddInclude(x => x.Role);
            AddInclude(x => x.PersonalProfile!);

            AddCriteria(x => x.PublicId == query.Id);
        }
    }
}
