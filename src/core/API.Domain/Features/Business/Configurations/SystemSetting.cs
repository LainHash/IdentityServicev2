using API.Domain.Common.Entities;
using API.Domain.Features.Business.Configurations.Enums;

namespace API.Domain.Features.Business.Configurations
{
    public partial class SystemSetting : AuditableEntity
    {
        public string Key { get; private set; } = string.Empty;
        public string Value { get; private set; } = string.Empty;
        public string? DefaultValue { get; private set; }
        public SettingDataType DataType { get; private set; } = SettingDataType.String;
        public string Group { get; private set; } = SettingGroups.General;
        public string? Description { get; private set; }
        public bool IsPublic { get; private set; }
        public bool IsSystem { get; private set; }
        public bool IsEncrypted { get; private set; }
    }

    public partial class SystemSetting
    {
        private SystemSetting() { }

        public SystemSetting(
            string key,
            string value,
            SettingDataType dataType,
            string group,
            string? defaultValue = null,
            string? description = null,
            bool isPublic = false,
            bool isSystem = false,
            bool isEncrypted = false)
        {
            Key = key.Trim();
            Value = value;
            DataType = dataType;
            Group = string.IsNullOrWhiteSpace(group) ? SettingGroups.General : group.Trim();
            DefaultValue = defaultValue ?? value;
            Description = description;
            IsPublic = isPublic;
            IsSystem = isSystem;
            IsEncrypted = isEncrypted;
        }

        public static SystemSetting Create(
            string key,
            string value,
            SettingDataType dataType = SettingDataType.String,
            string group = SettingGroups.General,
            string? defaultValue = null,
            string? description = null,
            bool isPublic = false,
            bool isSystem = false,
            bool isEncrypted = false)
        {
            return new SystemSetting(key, value, dataType, group, defaultValue, description, isPublic, isSystem, isEncrypted);
        }

        public SystemSetting UpdateValue(string newValue)
        {
            Value = newValue;
            return this;
        }

        public SystemSetting ResetToDefault()
        {
            if (DefaultValue is not null)
            {
                Value = DefaultValue;
            }
            return this;
        }

        public SystemSetting UpdateMetadata(string? description, string group, bool isPublic)
        {
            Description = description;
            Group = string.IsNullOrWhiteSpace(group) ? SettingGroups.General : group.Trim();
            IsPublic = isPublic;
            return this;
        }
    }
}
