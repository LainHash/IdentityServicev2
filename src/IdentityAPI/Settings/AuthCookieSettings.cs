namespace IdentityAPI.Settings
{
    public sealed class AuthCookieSettings
    {
        public const string SectionName = "AuthCookie";

        public string AccessTokenCookieName { get; init; } = "access_token";
        public string RefreshTokenCookieName { get; init; } = "refresh_token";

        public string RefreshTokenPath { get; init; } = "/api/identity/Authentication";
        public string? Domain { get; init; }
        public SameSiteMode SameSite { get; init; } = SameSiteMode.Lax;

        public bool Secure { get; init; } = true;
    }
}
