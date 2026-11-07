using API.Application.Features.Business.Audit;

namespace API.Infrastructure.Features.Business.Audit.Services
{
    internal class AuditContext : IAuditContext
    {
        public Guid? UserPublicId { get; set; }
        public string? IpAddress { get; set; }
    }
}
