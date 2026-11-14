using API.Contract.DTOs.Personnel.Employees;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Personnel.Employees.Queries.GetById
{
    internal class GetEmployeeByIdQueryHandler(IEmployeeService employeeService)
                : IRequestHandler<GetEmployeeByIdQuery, Result<EmployeeDetailResponse>>
    {
        private readonly IEmployeeService _employeeService = employeeService;

        public async Task<Result<EmployeeDetailResponse>> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            var specification = new GetEmployeeByIdSpecification(request);
            return await _employeeService.GetByIdAsync(specification, cancellationToken);
        }
    }
}
