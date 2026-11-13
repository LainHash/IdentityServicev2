using API.Domain.Extensions;
using API.Domain.Features.Identity.Users;
using API.Domain.Features.Personnel.Positions;
using API.Domain.Features.Personnel.Employees.Enums;
using API.Domain.Common.Entities;

namespace API.Domain.Features.Personnel.Employees
{
    public partial class Employee : SoftDeletableEntity
    {
        private readonly long EmployeeSequence;
        public string Code => EmployeeSequence.ToBase36();

        public long UserId { get; private set; }
        public User User { get; private set; } = null!;

        public long PositionId { get; private set; }
        public Position Position { get; private set; } = null!;

        public Guid? BranchPublicId { get; private set; }

        public DateTime HireDate { get; private set; }
        public DateTime? TerminationDate { get; private set; }

        public EmployeeStatus Status { get; private set; }
    }

    public partial class Employee
    {
        public Employee() { }

        public Employee(
            long userId,
            long positionId,
            Guid? branchId = null,
            DateTime? hireDate = null,
            EmployeeStatus status = EmployeeStatus.Active,
            DateTime? terminationDate = null)
        {
            UserId = userId;
            PositionId = positionId;
            BranchPublicId = branchId;
            HireDate = hireDate ?? DateTime.UtcNow;
            Status = status;
            TerminationDate = terminationDate;
        }

        public Employee SetUser(long userId)
        {
            UserId = userId;
            return this;
        }

        public Employee SetPosition(long positionId)
        {
            PositionId = positionId;
            return this;
        }

        public Employee SetBranch(Guid? branchId)
        {
            BranchPublicId = branchId;
            return this;
        }

        public Employee SetHireDate(DateTime hireDate)
        {
            HireDate = hireDate;
            return this;
        }

        public Employee SetStatus(EmployeeStatus status)
        {
            Status = status;
            return this;
        }

        public Employee SetTerminationDate(DateTime? terminationDate)
        {
            TerminationDate = terminationDate;
            return this;
        }
    }
}
