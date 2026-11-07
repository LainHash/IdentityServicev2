using API.Domain.Common.Enums;

namespace API.Contract.DTOs.Business.AuditLogs
{
    public class AuditLogResponse : BaseDTOResponse
    {
        public Guid? UserPublicId { get; set; }
        public AuditAction Action { get; set; }
        public string EntityName { get; set; } = null!;
        public Guid EntityId { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string? IpAddress { get; set; }
        public DateTime Timestamp { get; set; }
    };
}
