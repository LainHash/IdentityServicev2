using API.Domain.Common.Specifications;
using API.Domain.Extensions;
using API.Domain.Features.Personnel.Positions;

namespace API.Application.Features.Personnel.Positions.Queries.GetAll
{
    public class GetAllPositionsSpecification : BaseSpecification<Position>
    {
        public GetAllPositionsSpecification(GetAllPositionsQuery query)
        {
            EnableSoftDeleteFilter();
            AddInclude(x => x.Department);

            ApplyDateRange(query.From, query.To, p => p.CreatedAt);

            if (query.DepartmentId.HasValue && query.DepartmentId.Value != Guid.Empty)
            {
                AddCriteria(p => p.Department.PublicId == query.DepartmentId.Value);
            }

            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                var keyword = query.Keyword.Trim();
                AddCriteria(p => keyword.LikeContains(p.Name) ||
                                 keyword.LikeContains(p.Code) ||
                                 (p.Description != null && keyword.LikeContains(p.Description)) ||
                                 keyword.LikeContains(p.Department.Name) ||
                                 keyword.LikeContains(p.Department.Code));

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
                case "department":
                case "departmentname":
                    if (query.IsAscending)
                        ApplyOrderBy(p => p.Department.Name);
                    else
                        ApplyOrderByDescending(p => p.Department.Name);
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

