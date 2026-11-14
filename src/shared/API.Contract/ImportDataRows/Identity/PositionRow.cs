namespace API.Contract.ImportDataRows.Identity
{
    public class PositionRow
    {
        public Guid PublicId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string DepartmentCode { get; set; } = string.Empty;
        public string? DepartmentName { get; set; }
        public string? Description { get; set; }
    }
}

