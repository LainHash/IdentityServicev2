using API.Contract.DTOs.Personnel.Employees;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Personnel.Employees.Queries.GetById
{
    public record GetEmployeeByIdQuery(Guid Id)
        : IRequest<Result<EmployeeDetailResponse>>
    {
    }
}
