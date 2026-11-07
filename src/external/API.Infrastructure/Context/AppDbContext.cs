using API.Application.Features.Business.Audit;
using API.Contract.Settings;
using API.Domain.Entities.Identity.OtpVerifications;
using API.Domain.Features.Business.Audit;
using API.Domain.Features.Business.Configurations;
using API.Domain.Features.Identity.Roles;
using API.Domain.Features.Identity.UserResfreshTokens;
using API.Domain.Features.Identity.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API.Infrastructure.Context
{
    public partial class AppDbContext : DbContext
    {
        public DbSet<AuditLog> AuditLogs { get; set; } = null!;
        public DbSet<SystemSetting> SystemSettings { get; set; } = null!;

        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; } = null!;
        public DbSet<OtpVerification> OtpVerifications { get; set; } = null!;

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
