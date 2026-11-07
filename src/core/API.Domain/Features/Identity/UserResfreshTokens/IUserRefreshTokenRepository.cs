using API.Domain.Common.Repositories;

namespace API.Domain.Features.Identity.UserResfreshTokens
{
    public interface IUserRefreshTokenRepository : IRepository<UserRefreshToken>
    {
        Task<UserRefreshToken?> FindActiveByTokenHashAsync(
            string tokenHash,
            CancellationToken cancellationToken = default);

        Task RevokeAllByUserIdAsync(
            long userId,
            CancellationToken cancellationToken = default);
    }
}
