using API.Application.Features.Business.Audit;
using API.Contract.Settings;
using API.Domain.Features.Business.Audit;
using API.Domain.Features.Business.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Infrastructure.Context
{
    public partial class AppDbContext : DbContext
    {
        public DbSet<AuditLog> AuditLogs { get; set; } = null!;
        public DbSet<SystemSetting> SystemSettings { get; set; } = null!;

        // ── IAuditContext injected via constructor ────────────────────────────
        private readonly IAuditContext _auditContext;
        private readonly AuditLogSettings _auditLogSettings;

        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            IAuditContext auditContext,
            IOptions<AppSettings> settings)
            : base(options)
        {
            _auditContext = auditContext;
            _auditLogSettings = settings.Value.AuditLog;
        }
    }
}
