using API.Domain.Features.Identity.UserResfreshTokens;
using API.Infrastructure.Common.Repositories;
using API.Infrastructure.Contexts.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure.Features.Identity.UserRefreshTokens
{
    internal class UserRefreshTokenRepository(AppDbContext context)
        : Repository<UserRefreshToken>(context), IUserRefreshTokenRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<UserRefreshToken?> FindActiveByTokenHashAsync(
            string tokenHash,
            CancellationToken cancellationToken = default)
        {
            return await _context.UserRefreshTokens
                .Include(t => t.User)
                .FirstOrDefaultAsync(
                    t => t.TokenHash == tokenHash && !t.IsRevoked && t.ExpiresAt > DateTime.UtcNow,
                    cancellationToken);
        }

        public async Task RevokeAllByUserIdAsync(
            long userId,
            CancellationToken cancellationToken = default)
        {
            await _context.UserRefreshTokens
                .Where(t => t.UserId == userId && !t.IsRevoked)
                .ExecuteUpdateAsync(
                    setter => setter.SetProperty(t => t.IsRevoked, true),
                    cancellationToken);
        }
    }
}
