using API.Domain.Common.Repositories;

namespace API.Domain.Features.Business.Audit
{
    public interface IAuditLogRepository : IRepository<AuditLog>
    {
        Task PurgeOldLogsAsync(DateTime cutoff, CancellationToken cancellationToken = default);
    }
}
