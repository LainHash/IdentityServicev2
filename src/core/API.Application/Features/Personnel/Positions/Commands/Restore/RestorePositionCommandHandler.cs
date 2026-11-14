using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Personnel.Positions.Commands.Restore
{
    internal class RestorePositionCommandHandler(IPositionService positionService)
        : IRequestHandler<RestorePositionCommand, Result>
    {
        private readonly IPositionService _positionService = positionService;

        public async Task<Result> Handle(
            RestorePositionCommand request,
            CancellationToken cancellationToken)
        {
            return await _positionService.RestoreAsync(request, cancellationToken);
        }
    }
}

