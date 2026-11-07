using API.Domain.Common.Specifications;
using API.Domain.Extensions;
using API.Domain.Features.Identity.Roles;

namespace API.Application.Features.Identity.Roles.Queries.GetAll
{
    public class GetAllRolesSpecification
        : BaseSpecification<Role>
    {
        public GetAllRolesSpecification(GetAllRolesQuery query)
        {
            AddInclude(x => x.Users);

            AddCriteria(x => x.CreatedAt >= query.From && x.CreatedAt <= query.To);

            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                AddCriteria(p => query.Keyword.LikeContains(p.Name) ||
                    (p.Description != null && query.Keyword.LikeContains(p.Description)));

                if (Guid.TryParse(query.Keyword, out var publicId))
                {
                    AddCriteria(p => p.PublicId == publicId);
                }
            }

            switch (query.SortField)
            {
                case "name":
                    if (query.IsAscending)
                        ApplyOrderBy(p => p.Name);
                    else
                        ApplyOrderByDescending(p => p.Name);
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

