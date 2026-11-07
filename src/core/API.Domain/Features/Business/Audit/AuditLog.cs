using API.Domain.Common.Entities;
using API.Domain.Common.Enums;

namespace API.Domain.Features.Business.Audit
{
    public class AuditLog : Entity
    {
        public Guid? UserPublicId { get; private set; }

        public AuditAction Action { get; private set; }

        public string EntityName { get; private set; } = string.Empty;

        public Guid EntityId { get; private set; }

        public string? OldValues { get; private set; }

        public string? NewValues { get; private set; }

        public string? IpAddress { get; private set; }

        public DateTime Timestamp { get; private set; }

        private AuditLog() { }

        public static AuditLog Create(
            Guid? userPublicId,
            AuditAction action,
            string entityName,
            Guid entityId,
            string? oldValues,
            string? newValues,
            string? ipAddress,
            DateTime timestamp)
        {
            return new AuditLog
            {
                UserPublicId = userPublicId,
                Action = action,
                EntityName = entityName,
                EntityId = entityId,
                OldValues = oldValues,
                NewValues = newValues,
                IpAddress = ipAddress,
                Timestamp = timestamp
            };
        }
    }
}
