using API.Contract.DTOs.Personnel.Positions;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Personnel.Positions.Commands.Update
{
    public record UpdatePositionCommand(Guid Id, UpdatePositionRequest Body)
        : IRequest<Result<PositionResponse>>;
}

