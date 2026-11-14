using API.Domain.Common.Specifications;
using API.Domain.Extensions;
using API.Domain.Features.Personnel.Departments;

namespace API.Application.Features.Personnel.Departments.Queries.GetAll
{
    public class GetAllDepartmentsSpecification : BaseSpecification<Department>
    {
        public GetAllDepartmentsSpecification(GetAllDepartmentsQuery query)
        {
            EnableSoftDeleteFilter();

            ApplyDateRange(query.From, query.To, p => p.CreatedAt);

            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                var keyword = query.Keyword.Trim();
                AddCriteria(p => keyword.LikeContains(p.Name) ||
                                 keyword.LikeContains(p.Code) ||
                                 (p.Description != null && keyword.LikeContains(p.Description)));

                if (Guid.TryParse(keyword, out var publicId))
                {
                    AddCriteria(p => p.PublicId == publicId);
                }
            }

            switch (query.SortField?.ToLower())
            {
                case "name":
                    if (query.IsAscending)
                        ApplyOrderBy(p => p.Name);
                    else
                        ApplyOrderByDescending(p => p.Name);
                    break;
                case "code":
                    if (query.IsAscending)
                        ApplyOrderBy(p => p.Code);
                    else
                        ApplyOrderByDescending(p => p.Code);
                    break;
                case "created_at":
                default:
                    if (query.IsAscending)
                        ApplyOrderBy(p => p.CreatedAt);
                    else
                        ApplyOrderByDescending(p => p.CreatedAt);
                    break;
            }

            ApplyPaging(query.Page, query.PageSize);
        }
    }
}

