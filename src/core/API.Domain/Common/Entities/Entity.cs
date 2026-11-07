namespace API.Domain.Common.Entities
{
    public abstract class Entity
    {
        public long Id { get; private set; }
        public Guid PublicId { get; private set; } = Guid.NewGuid();
    }

    public abstract class AuditableEntity : Entity
    {
        public DateTime CreatedAt { get; private set; }
        public long? CreatedBy { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public long? UpdatedBy { get; private set; }

        public void MarkCreated(DateTime now)
        {
            CreatedAt = now;
            UpdatedAt = now;
        }

        public void MarkUpdated(DateTime now)
        {
            UpdatedAt = now;
        }
    }

    public abstract class SoftDeletableEntity : AuditableEntity
    {
        public bool IsDeleted { get; private set; }
        public DateTime? DeletedAt { get; private set; }
        public long? DeletedBy { get; private set; }

        public void SoftDelete()
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
        }

        public void Restore()
        {
            IsDeleted = false;
            DeletedAt = null;
        }
    }
}
