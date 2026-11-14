using API.Contract.DTOs.Personnel.Positions;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Personnel.Positions.Commands.Create
{
    internal class CreatePositionCommandHandler(IPositionService positionService)
        : IRequestHandler<CreatePositionCommand, Result<PositionResponse>>
    {
        private readonly IPositionService _positionService = positionService;

        public async Task<Result<PositionResponse>> Handle(
            CreatePositionCommand request,
            CancellationToken cancellationToken)
        {
            return await _positionService.CreateAsync(request, cancellationToken);
        }
    }
}

