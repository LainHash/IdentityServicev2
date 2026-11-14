using API.Domain.Common.Specifications;
using API.Domain.Features.Personnel.Employees;
using Microsoft.EntityFrameworkCore;

namespace API.Application.Features.Personnel.Employees.Queries.GetById
{
    public class GetEmployeeByIdSpecification
        : BaseSpecification<Employee>
    {
        public GetEmployeeByIdSpecification(GetEmployeeByIdQuery query)
        {
            AddIncludeAggregator(x => x
                .Include(e => e.Position)
                .Include(e => e.User)
                    .ThenInclude(u => u.PersonalProfile));

            AddCriteria(x => x.PublicId == query.Id);
        }
    }
}
