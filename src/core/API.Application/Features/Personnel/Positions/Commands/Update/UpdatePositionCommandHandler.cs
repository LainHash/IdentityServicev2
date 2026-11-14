using API.Contract.DTOs.Personnel.Positions;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Personnel.Positions.Commands.Update
{
    internal class UpdatePositionCommandHandler(IPositionService positionService)
        : IRequestHandler<UpdatePositionCommand, Result<PositionResponse>>
    {
        private readonly IPositionService _positionService = positionService;

        public async Task<Result<PositionResponse>> Handle(
            UpdatePositionCommand request,
            CancellationToken cancellationToken)
        {
            return await _positionService.UpdateAsync(request, cancellationToken);
        }
    }
}

