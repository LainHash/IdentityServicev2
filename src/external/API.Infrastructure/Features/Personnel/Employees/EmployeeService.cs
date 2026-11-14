using API.Application.Features.Personnel.Employees;
using API.Application.Features.Personnel.Employees.Queries.GetAll;
using API.Application.Features.Personnel.Employees.Queries.GetById;
using API.Contract.DTOs.Personnel.Employees;
using API.Domain.Common.Models.Messages;
using API.Domain.Common.Models.Results;
using API.Domain.Features.Personnel.Employees;
using AutoMapper;
using System.Net;

namespace API.Infrastructure.Features.Personnel.Employees
{
    internal class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        private readonly IMapper _mapper;
        public EmployeeService(
            IEmployeeRepository employeeRepository,
            IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<PaginationResult<IEnumerable<EmployeeResponse>>> GetAllAsync(
            GetAllEmployeesSpecification specification,
            CancellationToken cancellationToken = default)
        {
            var employees = await _employeeRepository
                .ToListAsync(specification, cancellationToken);

            var totalItems = await _employeeRepository
                .CountAsync(specification, cancellationToken);

            var response = _mapper.Map<IEnumerable<EmployeeResponse>>(employees);
            return PaginationResult<IEnumerable<EmployeeResponse>>.Succeed(
                response,
                Success.Retrieved("Employees"),
                totalItems,
                specification.Skip,
                specification.Take);
        }

        public async Task<Result<EmployeeDetailResponse>> GetByIdAsync(
            GetEmployeeByIdSpecification specification,
            CancellationToken cancellationToken = default)
        {
            var employee = await _employeeRepository
                .FindAsync(specification, cancellationToken);
            if (employee is null)
            {
                return Result<EmployeeDetailResponse>
                    .Fail(Error.NotFound("Employee"), HttpStatusCode.NotFound);
            }

            var response = _mapper.Map<EmployeeDetailResponse>(employee);
            return Result<EmployeeDetailResponse>
                .Succeed(response, Success.Retrieved("Employee"));
        }
    }
}
