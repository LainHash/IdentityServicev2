using API.Domain.Common.Specifications;
using API.Domain.Extensions;
using API.Domain.Features.Business.Audit;

namespace API.Application.Features.Business.Audit.Queries.GetAll
{
    public class GetAllAuditLogsSpecification
        : BaseSpecification<AuditLog>
    {
        public GetAllAuditLogsSpecification(GetAllAuditLogsQuery query)
        {
            AddCriteria(x => x.Timestamp >= query.From && x.Timestamp <= query.To);

            if (!string.IsNullOrWhiteSpace(query.Keyword))
            {
                AddCriteria(a =>
                    query.Keyword.LikeContains(a.EntityName) ||
                    query.Keyword.LikeContains(nameof(a.Action)));
            }

            if (!string.IsNullOrWhiteSpace(query.EntityName))
            {
                AddCriteria(x => x.EntityName == query.EntityName);
            }

            if (query.UserPublicId.HasValue)
            {
                AddCriteria(x => x.UserPublicId == query.UserPublicId);
            }

            if (query.Actions is { Length: > 0 })
            {
                AddCriteria(x => query.Actions.Contains(x.Action));
            }

            switch (query.SortField)
            {
                case "timestamp":
                    if (query.IsAscending)
                        ApplyOrderBy(p => p.Timestamp);
                    else
                        ApplyOrderByDescending(p => p.Timestamp);
                    break;
                default:
                    if (query.IsAscending)
                        ApplyOrderBy(p => p.Timestamp);
                    else
                        ApplyOrderByDescending(p => p.Timestamp);
                    break;
            }

            ApplyPaging(query.Page, query.PageSize);
        }
    }
}
