using API.Application.Features.Business;
using API.Application.Features.Business.Bootstrap.Services;
using API.Contract.DTOs.Business.Bootstrap;
using API.Contract.Settings;
using API.Domain.Common.Models.Results;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;

namespace API.Infrastructure.Features.Business.Bootstrap
{
    internal class BootstrapService : IBootstrapService
    {
        private readonly IEnumerable<IDataSeeder> _seeders;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOptions<AppSettings> _appSettings;
        private readonly ILogger<BootstrapService> _logger;

        public BootstrapService(
            IEnumerable<IDataSeeder> seeders,
            IUnitOfWork unitOfWork,
            IOptions<AppSettings> appSettings,
            ILogger<BootstrapService> logger)
        {
            _seeders = seeders;
            _unitOfWork = unitOfWork;
            _appSettings = appSettings;
            _logger = logger;
        }

        public async Task<Result<BootstrapResponse>> SeedAsync(
            string? providedKey,
            CancellationToken cancellationToken = default)
        {
            var settings = _appSettings.Value.Bootstrap;

            if (string.IsNullOrWhiteSpace(settings.Key))
            {
                _logger.LogError("Bootstrap key is not configured in application settings.");
                return Result<BootstrapResponse>
                    .Fail("Bootstrap service is not configured on this server.", HttpStatusCode.InternalServerError);
            }

            if (string.IsNullOrWhiteSpace(providedKey) ||
                !string.Equals(settings.Key.Trim(), providedKey.Trim(), StringComparison.Ordinal))
            {
                _logger.LogWarning("Unauthorized bootstrap attempt with invalid key.");
                return Result<BootstrapResponse>
                    .Fail("Invalid or missing bootstrap key.", HttpStatusCode.Unauthorized);
            }

            var orderedSeeders = _seeders.OrderBy(s => s.Order).ToList();
            var pendingSeeders = new List<IDataSeeder>();

            foreach (var seeder in orderedSeeders)
            {
                if (await seeder.ShouldSeedAsync(cancellationToken))
                {
                    pendingSeeders.Add(seeder);
                }
            }

            if (pendingSeeders.Count == 0)
            {
                _logger.LogWarning("Bootstrap attempt rejected: All seeders have already run. One-time key is consumed.");
                return Result<BootstrapResponse>
                    .Fail("The system has already been bootstrapped. The one-time bootstrap key is permanently invalidated.", HttpStatusCode.Conflict);
            }

            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var seederResults = new List<SeederResult>();

                foreach (var seeder in pendingSeeders)
                {
                    var result = await seeder.SeedAsync(cancellationToken);
                    seederResults.Add(result);
                }

                await transaction.CommitAsync(cancellationToken);

                _logger.LogInformation(
                    "Bootstrap successfully completed: {Count} seeders executed.",
                    seederResults.Count);

                var roleResult = seederResults.FirstOrDefault(r => r.SeederName == "RoleSeeder");
                var settingResult = seederResults.FirstOrDefault(r => r.SeederName == "SystemSettingSeeder");

                var response = new BootstrapResponse(
                    Message: "System bootstrapped successfully. The one-time key is now consumed.",
                    SettingsCreatedCount: settingResult?.ItemsCreatedCount ?? 0,
                    Settings: settingResult?.ItemsCreated,
                    Seeders: seederResults.Select(r => new SeederSummary(r.SeederName, r.ItemsCreatedCount, r.ItemsCreated))
                );

                return Result<BootstrapResponse>
                    .Succeed(response, "System bootstrapped successfully.", HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogError(ex, "Error occurred during bootstrap seeding: {Message}", ex.Message);
                return Result<BootstrapResponse>
                    .Fail($"Bootstrap failed: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }
    }
}
