namespace API.Contract.DTOs.Identity.Users
{
    public class UserResponse : SoftDeletableResponse
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }

        public string RoleName { get; set; } = string.Empty;
    }
}
