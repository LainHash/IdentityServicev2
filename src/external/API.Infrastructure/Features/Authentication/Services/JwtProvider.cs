using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using API.Application.Features.Authentication.Services;
using API.Contract.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace API.Infrastructure.Features.Authentication.Services
{
    internal class JwtProvider : IJwtProvider
    {
        private readonly RsaJwtKeyService _keyService;
        private readonly JwtSettings _jwtSettings;

        public JwtProvider(
            RsaJwtKeyService keyService,
            IOptions<AppSettings> options)
        {
            _keyService = keyService;
            _jwtSettings = options.Value.Jwt;
        }

        public string GenerateToken(Guid userId, string userName, string email, string role)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, userName),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(ClaimTypes.Role, role),
                new Claim("role", role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var credentials = new SigningCredentials(
                _keyService.GetSigningKey(),
                SecurityAlgorithms.RsaSha256);

            var expiryMinutes = _jwtSettings.ExpiryMinutes > 0
                ? _jwtSettings.ExpiryMinutes
                : 60;

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        public string HashToken(string rawToken)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawToken));
            return Convert.ToHexString(bytes).ToLowerInvariant();
        }

        public string GeneratePasswordResetToken(long userId)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim("purpose", "password_reset")
            };

            var credentials = new SigningCredentials(
                _keyService.GetSigningKey(),
                SecurityAlgorithms.RsaSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool TryValidatePasswordResetToken(string token, out long userId)
        {
            userId = 0;
            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = _keyService.GetValidationKey(),
                    ValidateIssuer = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _jwtSettings.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var handler = new JwtSecurityTokenHandler();
                var principal = handler.ValidateToken(token, validationParameters, out _);

                var purposeClaim = principal.FindFirst("purpose")?.Value;
                if (purposeClaim != "password_reset")
                    return false;

                var subClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                if (!long.TryParse(subClaim, out userId))
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
