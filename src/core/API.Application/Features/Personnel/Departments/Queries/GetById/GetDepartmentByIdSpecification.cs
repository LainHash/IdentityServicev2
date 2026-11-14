using API.Domain.Common.Specifications;
using API.Domain.Features.Personnel.Departments;

namespace API.Application.Features.Personnel.Departments.Queries.GetById
{
    public class GetDepartmentByIdSpecification : BaseSpecification<Department>
    {
        public GetDepartmentByIdSpecification(GetDepartmentByIdQuery query)
        {
            EnableSoftDeleteFilter();
            AddInclude(x => x.Positions);
            AddCriteria(x => x.PublicId == query.Id);
        }
    }
}

