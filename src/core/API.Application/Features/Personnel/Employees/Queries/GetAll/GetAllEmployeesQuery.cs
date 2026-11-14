using API.Contract.DTOs.Personnel.Employees;
using API.Domain.Common.Models;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Personnel.Employees.Queries.GetAll
{
    public record GetAllEmployeesQuery(Guid? PositionId)
        : PaginationQuery, IRequest<PaginationResult<IEnumerable<EmployeeResponse>>>
    {
    }
}
