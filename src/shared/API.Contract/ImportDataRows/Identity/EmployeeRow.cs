namespace API.Contract.ImportDataRows.Identity
{
    public class EmployeeRow
    {
        public Guid PublicId { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public string? PositionCode { get; set; }
        public string? PositionName { get; set; }
        public Guid? PositionPublicId { get; set; }
        public Guid? BranchPublicId { get; set; }
        public string? HireDate { get; set; }
        public string Status { get; set; } = "Active";
    }
}
