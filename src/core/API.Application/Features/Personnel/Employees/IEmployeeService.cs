using API.Application.Features.Personnel.Employees.Queries.GetAll;
using API.Application.Features.Personnel.Employees.Queries.GetById;
using API.Contract.DTOs.Personnel.Employees;
using API.Domain.Common.Models.Results;

namespace API.Application.Features.Personnel.Employees
{
    public interface IEmployeeService
    {
        Task<PaginationResult<IEnumerable<EmployeeResponse>>> GetAllAsync(
            GetAllEmployeesSpecification specification,
            CancellationToken cancellationToken = default);

        Task<Result<EmployeeDetailResponse>> GetByIdAsync(
            GetEmployeeByIdSpecification specification,
            CancellationToken cancellationToken = default);
    }
}
