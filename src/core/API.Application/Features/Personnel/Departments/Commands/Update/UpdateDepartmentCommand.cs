using API.Contract.DTOs.Personnel.Departments;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Personnel.Departments.Commands.Update
{
    public record UpdateDepartmentCommand(Guid Id, UpdateDepartmentRequest Body)
        : IRequest<Result<DepartmentResponse>>;
}

