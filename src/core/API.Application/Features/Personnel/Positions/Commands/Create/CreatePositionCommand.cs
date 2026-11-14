using API.Contract.DTOs.Personnel.Positions;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Personnel.Positions.Commands.Create
{
    public record CreatePositionCommand(CreatePositionRequest Body)
        : IRequest<Result<PositionResponse>>;
}

