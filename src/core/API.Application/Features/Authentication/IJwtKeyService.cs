namespace API.Application.Features.Authentication
{
    public interface IJwtKeyService
    {
        string KeyId { get; }
        string Algorithm { get; }
        string GetPublicKeyPem();
        (string ModulusBase64Url, string ExponentBase64Url) GetPublicKeyParameters();
    }
}

