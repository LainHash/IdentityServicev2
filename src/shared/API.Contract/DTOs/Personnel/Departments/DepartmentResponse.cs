namespace API.Contract.DTOs.Personnel.Departments
{
    public class DepartmentResponse : SoftDeletableResponse
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
