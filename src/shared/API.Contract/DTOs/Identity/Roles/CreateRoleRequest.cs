namespace API.Contract.DTOs.Identity.Roles
{
    public record CreateRoleRequest(
        string Name,
        string? Description);
}

