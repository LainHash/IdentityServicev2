namespace API.Contract.DTOs.Personnel.Departments
{
    public class UpdateDepartmentRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Code { get; set; }
        public string? Description { get; set; }
    }
}
