using API.Contract.DTOs.Authentication;
using API.Contract.Settings;
using IdentityAPI.Settings;

namespace IdentityAPI.Extensions
{
    public static class AuthCookieExtensions
    {
        public static void SetAuthCookies(
            this HttpResponse response,
            AuthenticationResponse auth,
            AuthCookieSettings cookieSettings,
            JwtSettings jwtSettings)
        {
            var now = DateTimeOffset.UtcNow;
            var isHttps = response.HttpContext.Request.IsHttps;

            response.Cookies.Append(
                cookieSettings.AccessTokenCookieName,
                auth.AccessToken,
                BuildOptions(
                    cookieSettings,
                    path: "/",
                    expires: now.AddMinutes(jwtSettings.ExpiryMinutes),
                    isHttps: isHttps));

            response.Cookies.Append(
                cookieSettings.RefreshTokenCookieName,
                auth.RefreshToken,
                BuildOptions(
                    cookieSettings,
                    path: cookieSettings.RefreshTokenPath,
                    expires: now.AddDays(jwtSettings.RefreshTokenExpiryDays),
                    isHttps: isHttps));
        }

        public static void ClearAuthCookies(
            this HttpResponse response,
            AuthCookieSettings cookieSettings)
        {
            var isHttps = response.HttpContext.Request.IsHttps;

            response.Cookies.Delete(
                cookieSettings.AccessTokenCookieName,
                BuildOptions(cookieSettings, path: "/", expires: null, isHttps: isHttps));

            response.Cookies.Delete(
                cookieSettings.RefreshTokenCookieName,
                BuildOptions(cookieSettings, path: cookieSettings.RefreshTokenPath, expires: null, isHttps: isHttps));
        }

        private static CookieOptions BuildOptions(
            AuthCookieSettings cookieSettings,
            string path,
            DateTimeOffset? expires,
            bool isHttps = false)
        {
            return new CookieOptions
            {
                HttpOnly = true,
                Secure = isHttps && (cookieSettings.Secure || cookieSettings.SameSite == SameSiteMode.None),
                SameSite = cookieSettings.SameSite,
                Domain = string.IsNullOrWhiteSpace(cookieSettings.Domain) ? null : cookieSettings.Domain,
                Path = path,
                Expires = expires,
                IsEssential = true
            };
        }
    }
}
