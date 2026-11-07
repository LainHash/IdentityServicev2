using API.Application.Features.Authentication;
using API.Application.Features.Authentication.Services;
using API.Application.Features.Business;
using API.Application.Features.Business.Audit;
using API.Application.Features.Business.Bootstrap.Services;
using API.Application.Features.Business.Email;
using API.Application.Features.Business.Import;
using API.Application.Features.Identity.OtpVerifications;
using API.Application.Features.Identity.Roles;
using API.Application.Features.Identity.Users;
using API.Contract.Settings;
using API.Domain.Common.Repositories;
using API.Domain.Features.Business.Audit;
using API.Infrastructure.Common.Repositories;
using API.Infrastructure.Common.Services;
using API.Infrastructure.Context;
using API.Infrastructure.Features.Authentication;
using API.Infrastructure.Features.Authentication.Services;
using API.Infrastructure.Features.Business.Audit;
using API.Infrastructure.Features.Business.Audit.Services;
using API.Infrastructure.Features.Business.Bootstrap;
using API.Infrastructure.Features.Business.Email;
using API.Infrastructure.Features.Business.Email.Background;
using API.Infrastructure.Features.Business.Import;
using API.Infrastructure.Features.Identity.OtpVerifications;
using API.Infrastructure.Features.Identity.Roles;
using API.Infrastructure.Features.Identity.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace API.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // ── AppSettings ──────────────────────────────────────────────────
            services.AddOptions<AppSettings>()
                    .Bind(configuration)
                    .ValidateOnStart();

            // ── Database ─────────────────────────────────────────────────────
            var dbSettings = configuration
                .GetSection("Database")
                .Get<DatabaseSettings>();
            var connectionString = dbSettings?.ConnectionString ?? configuration["DbConnectionString"];

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(
                    connectionString,
                    sqlOptions => sqlOptions.MigrationsAssembly(
                        typeof(AppDbContext).Assembly.FullName)));

            // ── AutoMapper ───────────────────────────────────────────────────
            services.AddAutoMapper(cfg => cfg.AddMaps(typeof(DependencyInjection).Assembly));

            // ── Repositories ─────────────────────────────────────────────────
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            var assembly = typeof(AuditLogRepository).Assembly;

            foreach (var type in assembly.GetTypes())
            {
                if (!type.IsClass || type.IsAbstract)
                    continue;

                if (!type.Name.EndsWith("Repository"))
                    continue;

                foreach (var iface in type.GetInterfaces())
                {
                    if (iface.Name.EndsWith("Repository"))
                    {
                        services.AddScoped(iface, type);
                    }
                }
            }

            //── Sheet Importers(Strategy Pattern) ───────────────────────────
            foreach (var type in assembly.GetTypes())
            {
                if (!type.IsClass || type.IsAbstract)
                    continue;

                if (typeof(ISheetImporter).IsAssignableFrom(type))
                {
                    services.AddScoped(typeof(ISheetImporter), type);
                }
            }

            // ── Data Seeders (Strategy Pattern) ───────────────────────────────
            foreach (var type in assembly.GetTypes())
            {
                if (!type.IsClass || type.IsAbstract)
                    continue;

                if (typeof(IDataSeeder).IsAssignableFrom(type))
                {
                    services.AddScoped(typeof(IDataSeeder), type);
                }
            }

            // ── Audit Log ────────────────────────────────────────────────────
            services.AddHttpContextAccessor();
            services.AddScoped<IAuditContext, AuditContext>();
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();
            services.AddScoped<IAuditLogService, AuditLogService>();
            services.AddHostedService<AuditLogRetentionJob>();

            // ── Authentication & Security ────────────────────────────────────
            services.AddSingleton<RsaJwtKeyService>();
            services.AddSingleton<IJwtKeyService>(sp => sp.GetRequiredService<RsaJwtKeyService>());
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            // ── Background Jobs ──────────────────────────────────────────────
            services.AddSingleton<IEmailQueue, EmailQueue>();
            services.AddHostedService<EmailBackgroundWorker>();

            // ── Services ─────────────────────────────────────────────────────
            services.AddSingleton(TimeProvider.System);
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IImportService, ImportService>();
            services.AddScoped<IBootstrapService, BootstrapService>();
            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IOtpVerificationService, OtpVerificationService>();

            return services;
        }
    }
}
