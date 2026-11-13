using API.Domain.Features.Personnel.Positions;
using API.Domain.Common.Entities;

namespace API.Domain.Features.Personnel.Departments
{
    public partial class Department : SoftDeletableEntity
    {
        public string Name { get; private set; } = null!;
        public string? Description { get; private set; }

        public string DepartmentCode { get; private set; } = null!;

        public ICollection<Position> Positions { get; private set; } = [];
    }

    public partial class Department
    {
        private Department() { }
    }
}
