using API.Domain.Features.Personnel.Employees.Enums;

namespace API.Contract.DTOs.Personnel.Employees
{
    public class EmployeeResponse : SoftDeletableResponse
    {
        public string Code { get; set; } = null!;
        public string PositionName { get; set; } = null!;
        public string PositionCode { get; set; } = null!;
        public Guid? BranchPublicId { get; set; }

        public DateTime HireDate { get; set; }
        public DateTime? TerminationDate { get; set; }

        public EmployeeStatus Status { get; set; }
    }
}
