namespace API.Contract.Settings
{
    public sealed class BootstrapSettings
    {
        public string Key { get; init; } = string.Empty;
        public string AdminUserName { get; init; } = string.Empty;
        public string AdminEmail { get; init; } = string.Empty;
        public string AdminPassword { get; init; } = string.Empty;
    }
}
