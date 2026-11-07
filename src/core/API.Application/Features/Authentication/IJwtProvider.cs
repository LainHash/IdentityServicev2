namespace API.Application.Features.Authentication
{
    public interface IJwtProvider
    {
        string GenerateAccessToken(Guid userPublicId, string email, IEnumerable<string> roles);
        string GenerateRefreshToken();
    }
}

