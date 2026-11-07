using API.Application.Features.Business.Audit.Queries.GetAll;
using API.Contract.DTOs.Business.AuditLogs;
using API.Domain.Common.Models.Results;

namespace API.Application.Features.Business.Audit
{
    public interface IAuditLogService
    {
        Task<PaginationResult<IEnumerable<AuditLogResponse>>> GetAllAsync(
            GetAllAuditLogsSpecification specification,
            CancellationToken cancellationToken = default);

        Task PurgeOldLogsAsync(int retentionDays, CancellationToken cancellationToken = default);
    }
}
