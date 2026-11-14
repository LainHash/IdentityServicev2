using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Personnel.Positions.Commands.Delete
{
    public record DeletePositionCommand(Guid Id)
        : IRequest<Result>;
}

