using API.Domain.Common.Entities;
using API.Domain.Entities.Identity.OtpVerifications;
using API.Domain.Features.Identity.PersonalProfiles;
using API.Domain.Features.Identity.Roles;
using API.Domain.Features.Identity.UserResfreshTokens;

namespace API.Domain.Features.Identity.Users
{
    public partial class User : SoftDeletableEntity
    {
        public string UserName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;

        public bool IsConfirmed { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsLocked { get; private set; }

        public long RoleId { get; private set; }
        public Role Role { get; private set; } = null!;

        public PersonalProfile? PersonalProfile { get; private set; }
        public ICollection<UserRefreshToken> RefreshTokens { get; private set; } = [];
        public ICollection<OtpVerification> OtpVerifications { get; private set; } = [];
    }

    public partial class User
    {
        public User() { }

        public User(
            string userName, 
            string email, 
            string passwordHash, 
            long roleId, 
            bool isActive = true, 
            bool isConfirmed = false)
        {
            UserName = userName;
            Email = email;
            PasswordHash = passwordHash;
            RoleId = roleId;
            IsActive = isActive;
            IsConfirmed = isConfirmed;
        }

        public static User Create(
            string userName, 
            string email, 
            string passwordHash, 
            long roleId, 
            bool isActive = true, 
            bool isConfirmed = false)
        {
            return new User(userName, email, passwordHash, roleId, isActive, isConfirmed);
        }

        public static User CreateForEmployee(string userName, string email, string passwordHash, long roleId)
        {
            return new User(userName, email, passwordHash, roleId, isActive: true, isConfirmed: true);
        }

        public User UpdateDetails(string userName, string email, long roleId, bool isActive, bool isConfirmed)
        {
            UserName = userName;
            Email = email;
            RoleId = roleId;
            IsActive = isActive;
            IsConfirmed = isConfirmed;
            return this;
        }

        public User SetConfirmed(bool isConfirmed)
        {
            IsConfirmed = isConfirmed;
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
            IsConfirmed = true;
        }
    }
}
