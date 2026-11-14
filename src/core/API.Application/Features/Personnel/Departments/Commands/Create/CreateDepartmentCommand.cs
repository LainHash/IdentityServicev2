using API.Contract.DTOs.Personnel.Departments;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Personnel.Departments.Commands.Create
{
    public record CreateDepartmentCommand(CreateDepartmentRequest Body)
        : IRequest<Result<DepartmentResponse>>;
}

