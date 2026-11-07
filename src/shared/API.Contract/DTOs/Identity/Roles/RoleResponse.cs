namespace API.Contract.DTOs.Identity.Roles
{
    public class RoleResponse : SoftDeletableResponse
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int UserCount { get; set; }
    }
}

