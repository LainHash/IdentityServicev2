using API.Contract.DTOs.Personnel.Positions;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Personnel.Positions.Queries.GetById
{
    public record GetPositionByIdQuery(Guid Id)
        : IRequest<Result<PositionDetailResponse>>;
}

