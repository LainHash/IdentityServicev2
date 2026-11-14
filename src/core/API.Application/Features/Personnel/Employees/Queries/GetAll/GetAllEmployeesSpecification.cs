using API.Domain.Common.Specifications;
using API.Domain.Extensions;
using API.Domain.Features.Personnel.Employees;
using Microsoft.EntityFrameworkCore;

namespace API.Application.Features.Personnel.Employees.Queries.GetAll
{
    public class GetAllEmployeesSpecification
        : BaseSpecification<Employee>
    {
        public GetAllEmployeesSpecification(GetAllEmployeesQuery query)
        {
            AddIncludeAggregator(x => x
                .Include(e => e.Position)
                .Include(e => e.User)
                    .ThenInclude(u => u.PersonalProfile));

            if (!string.IsNullOrEmpty(query.Keyword))
            {
                AddCriteria(x =>
                    query.Keyword.LikeEquals(x.Code) ||
                    query.Keyword.LikeEquals(x.Position.Code));
            }

            if (query.PositionId.HasValue)
            {
                AddCriteria(x => x.Position.PublicId == query.PositionId);
            }

            switch (query.SortField)
            {
                case "hire_date":
                    if (query.IsAscending)
                        ApplyOrderBy(x => x.HireDate);
                    else
                        ApplyOrderByDescending(x => x.HireDate);
                    break;
                case "created_at":
                default:
                    if (query.IsAscending)
                        ApplyOrderBy(x => x.CreatedAt);
                    else
                        ApplyOrderByDescending(x => x.CreatedAt);
                    break;
            }

            ApplyPaging(query.Page, query.PageSize);
        }
    }
}
