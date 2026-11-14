using API.Contract.DTOs.Personnel.Employees;
using API.Domain.Common.Models;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Personnel.Employees.Queries.GetAll
{
    internal class GetAllEmployeesQueryHandler(IEmployeeService employeeService)
                : IRequestHandler<GetAllEmployeesQuery, PaginationResult<IEnumerable<EmployeeResponse>>>
    {
        private readonly IEmployeeService _employeeService = employeeService;

        public async Task<PaginationResult<IEnumerable<EmployeeResponse>>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            var specification = new GetAllEmployeesSpecification(request);
            return await _employeeService.GetAllAsync(specification, cancellationToken);
        }
    }
}
