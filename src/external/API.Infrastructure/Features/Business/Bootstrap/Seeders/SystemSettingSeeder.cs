using API.Application.Features.Business;
using API.Application.Features.Business.Bootstrap.Services;
using API.Contract.Settings;
using API.Domain.Features.Business.Configurations;
using API.Domain.Features.Business.Configurations.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace API.Infrastructure.Features.Business.Bootstrap.Seeders
{
    internal class SystemSettingSeeder : IDataSeeder
    {
        public int Order => 3;

        private readonly ISystemSettingRepository _systemSettingRepository;
        private readonly IOptions<AppSettings> _appSettings;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SystemSettingSeeder> _logger;

        public SystemSettingSeeder(
            ISystemSettingRepository systemSettingRepository,
            IOptions<AppSettings> appSettings,
            IUnitOfWork unitOfWork,
            ILogger<SystemSettingSeeder> logger)
        {
            _systemSettingRepository = systemSettingRepository;
            _appSettings = appSettings;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        private IEnumerable<SystemSetting> GetDefaultSettings()
        {
            var retentionDays = _appSettings.Value.AuditLog?.RetentionDays.ToString() ?? "90";
            var auditEnabled = (_appSettings.Value.AuditLog?.Enabled ?? false).ToString().ToLowerInvariant();

            var auditLogRetentionDaysSetting = SystemSetting.Create(
                key: "audit_log.retention_days",
                value: retentionDays,
                dataType: SettingDataType.Integer,
                group: SettingGroups.AuditLog,
                defaultValue: "90",
                description: "Số ngày lưu trữ audit log trước khi tự động dọn dẹp (retention days)",
                isPublic: false,
                isSystem: true);

            var auditLogEnabledSetting = SystemSetting.Create(
                key: "audit_log.enabled",
                value: auditEnabled,
                dataType: SettingDataType.Boolean,
                group: SettingGroups.AuditLog,
                defaultValue: "false",
                description: "Bật hoặc tắt tính năng ghi nhận audit log hệ thống",
                isPublic: false,
                isSystem: true);

            return
            [
                auditLogRetentionDaysSetting,
                auditLogEnabledSetting
            ];
        }

        public async Task<bool> ShouldSeedAsync(CancellationToken cancellationToken = default)
        {
            foreach (var setting in GetDefaultSettings())
            {
                if (!await _systemSettingRepository.IsExistingKeyAsync(setting.Key, cancellationToken))
                    return true;
            }
            return false;
        }

        public async Task<SeederResult> SeedAsync(CancellationToken cancellationToken = default)
        {
            var defaultSettings = GetDefaultSettings();
            var settingsCreated = new List<string>();

            foreach (var setting in defaultSettings)
            {
                var exists = await _systemSettingRepository.IsExistingKeyAsync(setting.Key, cancellationToken);
                if (!exists)
                {
                    _systemSettingRepository.Add(setting);
                    settingsCreated.Add(setting.Key);
                }
            }

            if (settingsCreated.Count > 0)
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("SystemSettingSeeder: {Count} settings seeded: {Settings}", settingsCreated.Count, string.Join(", ", settingsCreated));
            }

            return new SeederResult(
                SeederName: "SystemSettingSeeder",
                ItemsCreatedCount: settingsCreated.Count,
                ItemsCreated: defaultSettings.Select(s => s.Key).ToList()
            );
        }
    }
}
