using API.Domain.Common.Entities;
using API.Domain.Features.Identity.Users;

namespace API.Domain.Features.Identity.UserResfreshTokens
{
    public class UserRefreshToken : BaseEntity
    {
        public string TokenHash { get; private set; } = string.Empty;
        public DateTime ExpiresAt { get; private set; }
        public bool IsRevoked { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public long UserId { get; private set; }
        public User User { get; private set; } = null!;

        public UserRefreshToken() { }

        public UserRefreshToken(long userId, string tokenHash, DateTime expiresAt)
        {
            UserId = userId;
            TokenHash = tokenHash;
            ExpiresAt = expiresAt;
            IsRevoked = false;
            CreatedAt = DateTime.UtcNow;
        }

        public bool IsExpired => DateTime.UtcNow > ExpiresAt;
        public bool IsActive => !IsRevoked && !IsExpired;

        public void Revoke()
        {
            IsRevoked = true;
        }
    }
}
