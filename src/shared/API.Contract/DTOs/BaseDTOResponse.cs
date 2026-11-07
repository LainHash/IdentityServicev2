namespace API.Contract.DTOs
{
    public class BaseDTOResponse
    {
        public Guid Id { get; set; }
    }

    public class AuditableResponse : BaseDTOResponse
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class SoftDeletableResponse : AuditableResponse
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
