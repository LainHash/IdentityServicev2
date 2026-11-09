using API.Domain.Common.Specifications;
using API.Domain.Extensions;
using API.Domain.Features.Guest.Customers;

namespace API.Application.Features.Guest.Queries.GetAll
{
    public class GetAllCustomersSpecification
        : BaseSpecification<Customer>
    {
        public GetAllCustomersSpecification(GetAllCustomersQuery query)
        {
            AddInclude(x => x.User);

            AddCriteria(x => x.CreatedAt >= query.From && x.CreatedAt <= query.To);

            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                AddCriteria(x => query.Keyword.LikeContains(x.Code) ||
                   query.Keyword.LikeContains(x.LoyaltyPoints.ToString()));

                if (Guid.TryParse(query.Keyword, out var publicId))
                {
                    AddCriteria(p => p.PublicId == publicId);
                }
            }

            switch (query.SortField)
            {
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
