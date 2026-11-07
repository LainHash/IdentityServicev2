namespace API.Contract.Settings
{
    public sealed class GoogleAuthSettings
    {
        public string ClientId { get; init; } = string.Empty;
        public string AndroidClientId { get; init; } = string.Empty;
        public string IosClientId { get; init; } = string.Empty;
    }
}
