namespace API.Contract.Settings
{
    public class AuditLogSettings
    {
        public const string SectionName = "AuditLogSettings";
        public int RetentionDays { get; init; } = 90;
        public bool Enabled { get; init; } = true;
    }
}
