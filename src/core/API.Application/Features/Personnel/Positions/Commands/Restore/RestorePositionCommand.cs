using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Personnel.Positions.Commands.Restore
{
    public record RestorePositionCommand(Guid Id)
        : IRequest<Result>;
}

