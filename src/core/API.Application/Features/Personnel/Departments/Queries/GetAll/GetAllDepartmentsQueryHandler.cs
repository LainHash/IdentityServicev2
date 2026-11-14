using API.Contract.DTOs.Personnel.Departments;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Personnel.Departments.Queries.GetAll
{
    internal class GetAllDepartmentsQueryHandler(IDepartmentService departmentService)
        : IRequestHandler<GetAllDepartmentsQuery, PaginationResult<IEnumerable<DepartmentResponse>>>
    {
        private readonly IDepartmentService _departmentService = departmentService;

        public async Task<PaginationResult<IEnumerable<DepartmentResponse>>> Handle(
            GetAllDepartmentsQuery request,
            CancellationToken cancellationToken)
        {
            var specification = new GetAllDepartmentsSpecification(request);
            return await _departmentService.GetAllAsync(specification, cancellationToken);
        }
    }
}

