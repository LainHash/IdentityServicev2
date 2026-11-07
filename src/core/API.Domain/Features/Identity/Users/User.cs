using API.Domain.Common.Entities;
using API.Domain.Entities.Identity.OtpVerifications;
using API.Domain.Features.Identity.Roles;
using API.Domain.Features.Identity.UserResfreshTokens;
using API.Domain.Features.Identity.Users.Enums;

namespace API.Domain.Features.Identity.Users
{
    public partial class User : SoftDeletableEntity
    {
        public string UserName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;

        public bool IsActive { get; private set; }
        public bool IsLocked { get; private set; }

        public long RoleId { get; private set; }

        public Role Role { get; private set; } = null!;
        public ICollection<UserRefreshToken> RefreshTokens { get; private set; } = [];
        public ICollection<OtpVerification> OtpVerifications { get; private set; } = [];
    }

    public partial class User
    {
        public User() { }

        public User(string userName, string email, string passwordHash, long roleId, bool isActive)
        {
            UserName = userName;
            Email = email;
            PasswordHash = passwordHash;
            RoleId = roleId;
            IsActive = isActive;
        }

        public static User Create(string userName, string email, string passwordHash, long roleId, bool isActive = true)
        {
            return new User(userName, email, passwordHash, roleId, isActive);
        }

        public static User CreateForEmployee(string userName, string email, string passwordHash, long roleId)
        {
            return new User(userName, email, passwordHash, roleId, true);
        }

        public User UpdateDetails(string userName, string email, long roleId, bool isActive)
        {
            UserName = userName;
            Email = email;
            RoleId = roleId;
            IsActive = isActive;
            return this;
        }

        public User SetActive(bool isActive)
        {
            IsActive = isActive;
            return this;
        }

        public User ToggleActive()
        {
            IsActive = !IsActive;
            return this;
        }

        public User SetLocked(bool isLocked)
        {
            IsLocked = isLocked;
            return this;
        }

        public User ToggleLock()
        {
            IsLocked = !IsLocked;
            return this;
        }

        public User SetPasswordHash(string passwordHash)
        {
            PasswordHash = passwordHash;
            return this;
        }

        public User SetRole(long roleId)
        {
            RoleId = roleId;
            return this;
        }

        public void CompleteVerification()
        {
            IsActive = true;
        }
    }
}
