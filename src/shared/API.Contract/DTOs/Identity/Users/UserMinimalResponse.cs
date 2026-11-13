namespace API.Contract.DTOs.Identity.Users
{
    public class UserMinimalResponse : SoftDeletableResponse
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsConfirmed { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
    }
}
