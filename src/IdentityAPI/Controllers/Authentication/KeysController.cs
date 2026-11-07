using API.Application.Features.Authentication.Services;
using API.Contract.DTOs.Authentication;
using API.Contract.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace IdentityAPI.Controllers.Authentication
{
    [ApiController]
    [AllowAnonymous]
    public class KeysController(
        IJwtKeyService keyService,
        IOptions<AppSettings> appSettings) : ControllerBase
    {
        private readonly IJwtKeyService _keyService = keyService;
        private readonly JwtSettings _jwtSettings = appSettings.Value.Jwt;

        /// <summary>
        /// OpenID Connect Discovery Document (Cho phép các Microservice khác tự động tìm jwks_uri)
        /// </summary>
        [HttpGet(".well-known/openid-configuration")]
        [HttpGet("api/identity/.well-known/openid-configuration")]
        public IActionResult GetOpenIdConfiguration()
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var isViaGateway = Request.Path.StartsWithSegments("/api/identity");
            var jwksUri = isViaGateway
                ? $"{baseUrl}/api/identity/.well-known/jwks.json"
                : $"{baseUrl}/.well-known/jwks.json";

            var response = new OpenIdConfigurationResponse(
                Issuer: _jwtSettings.Issuer,
                JwksUri: jwksUri,
                IdTokenSigningAlgValuesSupported: [_keyService.Algorithm]);

            return Ok(response);
        }

        /// <summary>
        /// JSON Web Key Set (JWKS - RFC 7517) chứa RSA Public Key để xác thực chữ ký JWT
        /// </summary>
        [HttpGet(".well-known/jwks.json")]
        [HttpGet("api/identity/.well-known/jwks.json")]
        public IActionResult GetJwks()
        {
            var (modulus, exponent) = _keyService.GetPublicKeyParameters();

            var jwk = new JwkKeyDto(
                Kty: "RSA",
                Use: "sig",
                Alg: _keyService.Algorithm,
                Kid: _keyService.KeyId,
                N: modulus,
                E: exponent);

            return Ok(new JwksResponse([jwk]));
        }

        /// <summary>
        /// Phát hành RSA Public Key dưới định dạng PEM trực tiếp
        /// </summary>
        [HttpGet("api/identity/keys/public")]
        public IActionResult GetPublicKeyPem()
        {
            var response = new PublicKeyResponse(
                KeyId: _keyService.KeyId,
                Algorithm: _keyService.Algorithm,
                PublicKeyPem: _keyService.GetPublicKeyPem());

            return Ok(response);
        }
    }
}
