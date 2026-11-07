using API.Domain.Common.Entities;
using API.Domain.Common.Enums;
using API.Domain.Features.Business.Audit;
using API.Infrastructure.Features.Business.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace API.Infrastructure.Context
{
    public partial class AppDbContext
    {
        // ── Auto-set audit fields on SaveChanges ────────────────────────────
        public override int SaveChanges()
        {
            SetAuditFields();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetAuditFields();

            var auditEntries = _auditLogSettings.Enabled
                ? CaptureAuditEntries()
                : [];

            var result = await base.SaveChangesAsync(cancellationToken);

            if (auditEntries.Count > 0)
            {
                FinalizeAuditEntries(auditEntries);
                AuditLogs.AddRange(auditEntries.Select(e => (AuditLog)e));
                await base.SaveChangesAsync(cancellationToken);
            }

            return result;
        }

        // ── Internal helpers ────────────────────────────────────────────────
        private void SetAuditFields()
        {
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.MarkCreated(now);
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.MarkUpdated(now);
                }
            }
        }

        private List<AuditEntry> CaptureAuditEntries()
        {
            ChangeTracker.DetectChanges();

            var auditEntries = new List<AuditEntry>();
            var timestamp = DateTime.UtcNow;

            var trackedStates = new[] { EntityState.Added, EntityState.Modified, EntityState.Deleted };

            foreach (var entry in ChangeTracker.Entries<Entity>())
            {
                if (!trackedStates.Contains(entry.State)) continue;

                var auditEntry = new AuditEntry(entry)
                {
                    UserPublicId = _auditContext.UserPublicId,
                    IpAddress = _auditContext.IpAddress,
                    EntityName = entry.Metadata.ClrType.Name,
                    EntityId = entry.Entity.PublicId,
                    Timestamp = timestamp,
                    Action = entry.State switch
                    {
                        EntityState.Added => AuditAction.POST,
                        EntityState.Modified => AuditAction.PUT,
                        EntityState.Deleted => AuditAction.DELETE,
                        _ => AuditAction.UNKNOWN
                    }
                };

                foreach (var prop in entry.Properties)
                {
                    if (prop.Metadata.IsShadowProperty()) continue;

                    var propName = prop.Metadata.Name;

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.NewValues[propName] = prop.CurrentValue;
                            if (prop.Metadata.IsPrimaryKey())
                                auditEntry.IsTemporaryId = true;
                            break;

                        case EntityState.Deleted:
                            auditEntry.OldValues[propName] = prop.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (prop.IsModified)
                            {
                                auditEntry.OldValues[propName] = prop.OriginalValue;
                                auditEntry.NewValues[propName] = prop.CurrentValue;
                            }
                            break;
                    }
                }

                auditEntries.Add(auditEntry);
            }

            return auditEntries;
        }

        private static void FinalizeAuditEntries(List<AuditEntry> auditEntries)
        {
            foreach (var entry in auditEntries.Where(e => e.IsTemporaryId))
            {
                entry.EntityId = entry.DbEntry.Entity.PublicId;
                entry.NewValues[nameof(Entity.Id)] = entry.DbEntry.Entity.Id;
            }
        }
    }
}
