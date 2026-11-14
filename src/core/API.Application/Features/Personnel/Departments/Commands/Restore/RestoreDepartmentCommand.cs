using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Personnel.Departments.Commands.Restore
{
    public record RestoreDepartmentCommand(Guid Id)
        : IRequest<Result>;
}

