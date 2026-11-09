using API.Domain.Common.Specifications;
using API.Domain.Features.Guest.Customers;
using Microsoft.EntityFrameworkCore;

namespace API.Application.Features.Guest.Queries.GetByUserId
{
    public class GetCustomerByUserIdSpecification
        : BaseSpecification<Customer>
    {
        public GetCustomerByUserIdSpecification(GetCustomerByUserIdQuery query)
        {
            AddIncludeAggregator(x =>
                x.Include(c => c.User)
                    .ThenInclude(u => u.PersonalProfile));

            AddCriteria(x => x.User.PublicId == query.UserId);
        }
    }
}

