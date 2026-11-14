using API.Domain.Features.Personnel.Positions;
using API.Domain.Common.Entities;

namespace API.Domain.Features.Personnel.Departments
{
    public partial class Department : SoftDeletableEntity
    {
        public string Name { get; private set; } = null!;
        public string? Description { get; private set; }

        public string Code { get; private set; } = null!;

        public ICollection<Position> Positions { get; private set; } = [];
    }

    public partial class Department
    {
        public Department() { }

        public Department(string name, string departmentCode, string? description = null)
        {
            Name = name;
            Code = departmentCode;
            Description = description;
        }

        public Department Update(string name, string departmentCode, string? description = null)
        {
            Name = name;
            Code = departmentCode;
            Description = description;
            return this;
        }
    }
}
