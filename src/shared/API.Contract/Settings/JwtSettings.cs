namespace API.Contract.Settings
{
    public sealed class JwtSettings
    {
        public string Issuer { get; init; } = string.Empty;
        public string Audience { get; init; } = string.Empty;
        public int ExpiryMinutes { get; init; } = 60;
        public int RefreshTokenExpiryDays { get; init; } = 7;
        public string KeyId { get; init; } = "identity-rsa-key-1";
        public string PrivateKeyPath { get; init; } = "keys/private_key.pem";
        public string? PrivateKeyPem { get; init; }
    }
}
