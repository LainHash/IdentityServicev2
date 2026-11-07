using API.Domain.Common.Specifications;
using API.Domain.Extensions;
using API.Domain.Features.Identity.Users;
using Microsoft.EntityFrameworkCore;

namespace API.Application.Features.Identity.Users.Queries.GetAll
{
    public class GetAllUsersSpecification
        : BaseSpecification<User>
    {
        public GetAllUsersSpecification(GetAllUsersQuery query)
        {
            AddInclude(x => x.Role);

            AddCriteria(x => x.CreatedAt >= query.From && x.CreatedAt <= query.To);

            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                AddCriteria(p =>
                    query.Keyword.LikeContains(p.UserName) ||
                    query.Keyword.LikeContains(p.Email) ||
                    query.Keyword.LikeContains(p.Role.Name));
                if(Guid.TryParse(query.Keyword, out var publicId))
                {
                    AddCriteria(p => p.PublicId == publicId);
                }
            }

            switch (query.SortField)
            {
                case "created_at":
                    if (query.IsAscending)
                        ApplyOrderBy(p => p.CreatedAt);
                    else
                        ApplyOrderByDescending(p => p.CreatedAt);
                    break;
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
