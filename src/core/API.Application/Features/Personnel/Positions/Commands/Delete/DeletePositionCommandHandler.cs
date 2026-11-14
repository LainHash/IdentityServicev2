using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Personnel.Positions.Commands.Delete
{
    internal class DeletePositionCommandHandler(IPositionService positionService)
        : IRequestHandler<DeletePositionCommand, Result>
    {
        private readonly IPositionService _positionService = positionService;

        public async Task<Result> Handle(
            DeletePositionCommand request,
            CancellationToken cancellationToken)
        {
            return await _positionService.DeleteAsync(request, cancellationToken);
        }
    }
}

