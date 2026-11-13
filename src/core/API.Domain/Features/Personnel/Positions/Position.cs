using API.Domain.Features.Personnel.Departments;
using API.Domain.Features.Personnel.Employees;
using API.Domain.Common.Entities;

namespace API.Domain.Features.Personnel.Positions
{
    public partial class Position : SoftDeletableEntity
    {
        public string Name { get; private set; } = null!;
        public string? Description { get; private set; }

        public string PositionCode { get; private set; } = null!;

        public long DepartmentId { get; private set; }
        public Department Department { get; private set; } = null!;

        public ICollection<Employee> Employees { get; private set; } = [];
    }

    public partial class Position
    {
        public Position() { }

        public Position(string name, string positionCode, long departmentId, string? description = null)
        {
            Name = name;
            PositionCode = positionCode;
            DepartmentId = departmentId;
            Description = description;
        }

        public Position SetDepartment(long departmentId)
        {
            DepartmentId = departmentId;
            return this;
        }

        public Position Update(string name, string positionCode, string? description = null)
        {
            Name = name;
            PositionCode = positionCode;
            Description = description;
            return this;
        }
    }
}
