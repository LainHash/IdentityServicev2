using API.Contract.DTOs.Personnel.Positions;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Personnel.Positions.Queries.GetById
{
    internal class GetPositionByIdQueryHandler(IPositionService positionService)
        : IRequestHandler<GetPositionByIdQuery, Result<PositionDetailResponse>>
    {
        private readonly IPositionService _positionService = positionService;

        public async Task<Result<PositionDetailResponse>> Handle(
            GetPositionByIdQuery request,
            CancellationToken cancellationToken)
        {
            var specification = new GetPositionByIdSpecification(request);
            return await _positionService.GetByIdAsync(specification, cancellationToken);
        }
    }
}

