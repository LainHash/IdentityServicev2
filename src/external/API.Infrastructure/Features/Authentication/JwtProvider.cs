using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using API.Application.Features.Authentication;
using API.Contract.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace API.Infrastructure.Features.Authentication
{
    public sealed class JwtProvider(
        RsaJwtKeyService keyService,
        IOptions<AppSettings> appSettings) : IJwtProvider
    {
        private readonly RsaJwtKeyService _keyService = keyService;
        private readonly JwtSettings _jwtSettings = appSettings.Value.Jwt;

        public string GenerateAccessToken(
            Guid userPublicId,
            string email,
            IEnumerable<string> roles)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userPublicId.ToString()),
                new(ClaimTypes.NameIdentifier, userPublicId.ToString()),
                new(JwtRegisteredClaimNames.Email, email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles.Where(r => !string.IsNullOrWhiteSpace(r)).Distinct())
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
                claims.Add(new Claim("role", role));
            }

            var signingCredentials = new SigningCredentials(
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
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(randomBytes);
        }
    }
}

