using System.Security.Cryptography;
using API.Application.Features.Authentication;
using API.Contract.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace API.Infrastructure.Features.Authentication
{
    public sealed class RsaJwtKeyService : IJwtKeyService, IDisposable
    {
        private readonly RSA _rsa;
        private readonly string _publicKeyPem;
        private readonly string _modulusBase64Url;
        private readonly string _exponentBase64Url;

        public string KeyId { get; }
        public string Algorithm => SecurityAlgorithms.RsaSha256;

        public RsaJwtKeyService(IOptions<AppSettings> appSettings)
        {
            var jwtSettings = appSettings.Value.Jwt;
            KeyId = string.IsNullOrWhiteSpace(jwtSettings.KeyId)
                ? "identity-rsa-key-1"
                : jwtSettings.KeyId;

            _rsa = RSA.Create(2048);
            LoadOrGeneratePrivateKey(jwtSettings);

            _publicKeyPem = _rsa.ExportSubjectPublicKeyInfoPem();

            var publicParams = _rsa.ExportParameters(includePrivateParameters: false);
            _modulusBase64Url = Base64UrlEncoder.Encode(publicParams.Modulus!);
            _exponentBase64Url = Base64UrlEncoder.Encode(publicParams.Exponent!);
        }

        private void LoadOrGeneratePrivateKey(JwtSettings jwtSettings)
        {
            // 1. Ưu tiên đọc trực tiếp từ biến môi trường Jwt__PrivateKeyPem nếu có
            if (!string.IsNullOrWhiteSpace(jwtSettings.PrivateKeyPem))
            {
                var pem = jwtSettings.PrivateKeyPem.Replace("\\n", "\n");
                _rsa.ImportFromPem(pem);
                return;
            }

            // 2. Đọc từ file PrivateKeyPath (nếu chưa có thì tự động sinh cặp khóa RSA 2048-bit)
            var relativePath = string.IsNullOrWhiteSpace(jwtSettings.PrivateKeyPath)
                ? "keys/private_key.pem"
                : jwtSettings.PrivateKeyPath;

            var fullPrivatePath = Path.IsPathRooted(relativePath)
                ? relativePath
                : Path.Combine(Directory.GetCurrentDirectory(), relativePath);

            if (File.Exists(fullPrivatePath))
            {
                var privatePem = File.ReadAllText(fullPrivatePath);
                _rsa.ImportFromPem(privatePem);
                return;
            }

            var directory = Path.GetDirectoryName(fullPrivatePath);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var generatedPrivatePem = _rsa.ExportPkcs8PrivateKeyPem();
            var generatedPublicPem = _rsa.ExportSubjectPublicKeyInfoPem();

            File.WriteAllText(fullPrivatePath, generatedPrivatePem);

            var fullPublicPath = Path.Combine(
                directory ?? Directory.GetCurrentDirectory(),
                "public_key.pem");
            File.WriteAllText(fullPublicPath, generatedPublicPem);
        }

        public RsaSecurityKey GetSigningKey()
        {
            return new RsaSecurityKey(_rsa)
            {
                KeyId = KeyId
            };
        }

        public RsaSecurityKey GetValidationKey()
        {
            var publicParams = _rsa.ExportParameters(includePrivateParameters: false);
            return new RsaSecurityKey(publicParams)
            {
                KeyId = KeyId
            };
        }

        public string GetPublicKeyPem() => _publicKeyPem;

        public (string ModulusBase64Url, string ExponentBase64Url) GetPublicKeyParameters()
            => (_modulusBase64Url, _exponentBase64Url);

        public void Dispose()
        {
            _rsa.Dispose();
        }
    }
}

