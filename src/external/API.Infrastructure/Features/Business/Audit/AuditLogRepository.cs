using API.Domain.Features.Business.Audit;
using API.Infrastructure.Common.Repositories;
using API.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure.Features.Business.Audit
{
    internal class AuditLogRepository(AppDbContext context) 
        : Repository<AuditLog>(context), IAuditLogRepository
    {
        private readonly AppDbContext _context = context;

        public async Task PurgeOldLogsAsync(
            DateTime cutoff, 
            CancellationToken cancellationToken = default)
        {
            await _context.AuditLogs
                .Where(l => l.Timestamp < cutoff)
                .ExecuteDeleteAsync(cancellationToken);
        }
    }
}
