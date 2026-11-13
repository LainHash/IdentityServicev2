namespace API.Contract.ImportDataRows.Identity
{
    public class UserRow
    {
        public Guid PublicId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Password { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public bool IsConfirmed { get; set; } = true;
        public bool IsActive { get; set; } = true;
        public bool IsLocked { get; set; } = false;
    }
}

