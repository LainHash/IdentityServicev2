namespace API.Contract.DTOs.Identity.Roles
{
    public record UpdateRoleRequest(
        string Name,
        string? Description);
}

