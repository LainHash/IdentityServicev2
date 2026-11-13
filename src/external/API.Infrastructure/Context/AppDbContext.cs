using API.Application.Features.Business.Audit;
using API.Contract.Settings;
using API.Domain.Entities.Identity.OtpVerifications;
using API.Domain.Features.Business.Audit;
using API.Domain.Features.Business.Configurations;
using API.Domain.Features.Guest.CustomerLoyaltyTransactions;
using API.Domain.Features.Guest.Customers;
using API.Domain.Features.Identity.PersonalProfiles;
using API.Domain.Features.Identity.Roles;
using API.Domain.Features.Identity.UserResfreshTokens;
using API.Domain.Features.Identity.Users;
using API.Domain.Features.Personnel.Departments;
using API.Domain.Features.Personnel.Employees;
using API.Domain.Features.Personnel.Positions;
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
        public DbSet<PersonalProfile> PersonalProfiles { get; set; } = null!;

        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<CustomerLoyaltyTransaction> CustomerLoyaltyTransactions { get; set; } = null!;

        public DbSet<Department> Departments { get; set; } = null!;
        public DbSet<Position> Positions { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;

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
