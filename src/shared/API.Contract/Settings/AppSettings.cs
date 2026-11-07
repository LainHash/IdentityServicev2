namespace API.Contract.Settings
{
    public sealed class AppSettings
    {
        public string AppName { get; init; } = string.Empty;
        public string AppVersion { get; init; } = string.Empty;

        public AuditLogSettings AuditLog { get; init; } = new();
        public DatabaseSettings Database { get; init; } = new();
        public BootstrapSettings Bootstrap { get; init; } = new();
        public JwtSettings Jwt { get; init; } = new();
    }
}
