namespace API.Contract.ImportDataRows.Identity
{
    public class DepartmentRow
    {
        public Guid PublicId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string DepartmentCode { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}

