namespace API.Application.Features.Business.Audit
{
    public interface IAuditContext
    {
        Guid? UserPublicId { get; set; }
        string? IpAddress { get; set; }
    }
}
