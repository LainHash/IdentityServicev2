using API.Domain.Common.Entities;
using API.Domain.Common.Enums;
using API.Domain.Features.Business.Audit;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace API.Infrastructure.Features.Business.Audit
{
    internal sealed class AuditEntry(EntityEntry<Entity> dbEntry)
    {
        public EntityEntry<Entity> DbEntry { get; } = dbEntry;
        public Guid? UserPublicId { get; set; }
        public string? IpAddress { get; set; }
        public string EntityName { get; set; } = string.Empty;
        public Guid EntityId { get; set; }
        public AuditAction Action { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsTemporaryId { get; set; }
        public Dictionary<string, object?> OldValues { get; } = [];
        public Dictionary<string, object?> NewValues { get; } = [];

        public static implicit operator AuditLog(AuditEntry entry)
            => AuditLog.Create(
                userPublicId: entry.UserPublicId,
                action: entry.Action,
                entityName: entry.EntityName,
                entityId: entry.EntityId,
                oldValues: entry.OldValues.Count > 0
                    ? JsonSerializer.Serialize(entry.OldValues)
                    : null,
                newValues: entry.NewValues.Count > 0
                    ? JsonSerializer.Serialize(entry.NewValues)
                    : null,
                ipAddress: entry.IpAddress,
                timestamp: entry.Timestamp);
    }
}
