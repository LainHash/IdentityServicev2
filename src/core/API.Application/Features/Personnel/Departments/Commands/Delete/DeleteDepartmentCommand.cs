using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Personnel.Departments.Commands.Delete
{
    public record DeleteDepartmentCommand(Guid Id)
        : IRequest<Result>;
}

