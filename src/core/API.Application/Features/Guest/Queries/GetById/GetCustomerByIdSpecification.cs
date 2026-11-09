using API.Domain.Common.Specifications;
using API.Domain.Features.Guest.Customers;
using Microsoft.EntityFrameworkCore;

namespace API.Application.Features.Guest.Queries.GetById
{
    public class GetCustomerByIdSpecification
        : BaseSpecification<Customer>
    {
        public GetCustomerByIdSpecification(GetCustomerByIdQuery query)
        {
            AddIncludeAggregator(x =>
                x.Include(c => c.User)
                    .ThenInclude(u => u.PersonalProfile));

            AddCriteria(x => x.PublicId == query.Id);
        }
    }
}
