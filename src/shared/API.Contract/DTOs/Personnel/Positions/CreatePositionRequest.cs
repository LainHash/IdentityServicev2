namespace API.Contract.DTOs.Personnel.Positions
{
    public class CreatePositionRequest
    {
        public Guid DepartmentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Code { get; set; }
        public string? Description { get; set; }
    }
}
