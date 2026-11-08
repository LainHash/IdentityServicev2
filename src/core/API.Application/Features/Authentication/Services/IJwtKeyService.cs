namespace API.Application.Features.Authentication.Services
{
    public interface IJwtKeyService
    {
        string KeyId { get; }
        string Algorithm { get; }
        string GetPublicKeyPem();
        (string ModulusBase64Url, string ExponentBase64Url) GetPublicKeyParameters();
    }
}

