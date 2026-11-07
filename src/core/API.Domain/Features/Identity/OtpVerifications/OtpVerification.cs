using API.Domain.Common.Entities;
using API.Domain.Features.Identity.OtpVerifications.Enums;
using API.Domain.Features.Identity.Users;

namespace API.Domain.Entities.Identity.OtpVerifications
{
    public partial class OtpVerification : AuditableEntity
    {
        public long UserId { get; private set; }

        public OtpPurpose Purpose { get; private set; }

        public string CodeHash { get; private set; } = null!;

        public DateTime ExpiresAt { get; private set; }
        public DateTime? UsedAt { get; private set; }

        public int FailedAttempts { get; private set; }
        public bool IsAvailable { get; private set; }

        public User User { get; private set; } = null!;
    }

    public partial class OtpVerification
    {
        public OtpVerification() { }

        public OtpVerification(long userId, string codeHash, OtpPurpose otpPurpose)
        {
            UserId = userId;
            CodeHash = codeHash;
            Purpose = otpPurpose;
            ExpiresAt = DateTime.UtcNow.AddMinutes(15);
            IsAvailable = true;
        }

        public void Invalidate()
        {
            IsAvailable = false;
        }
        public void IncrementFailedAttempt()
        {
            FailedAttempts++;
        }

        public void MarkAsUsed()
        {
            UsedAt = DateTime.UtcNow;
        }

        public void Verify()
        {

        }
    }
}
