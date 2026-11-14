using API.Contract.DTOs.Personnel.Positions;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Personnel.Positions.Queries.GetAll
{
    internal class GetAllPositionsQueryHandler(IPositionService positionService)
        : IRequestHandler<GetAllPositionsQuery, PaginationResult<IEnumerable<PositionResponse>>>
    {
        private readonly IPositionService _positionService = positionService;

        public async Task<PaginationResult<IEnumerable<PositionResponse>>> Handle(
            GetAllPositionsQuery request,
            CancellationToken cancellationToken)
        {
            var specification = new GetAllPositionsSpecification(request);
            return await _positionService.GetAllAsync(specification, cancellationToken);
        }
    }
}

