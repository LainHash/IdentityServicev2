using API.Contract.DTOs.Personnel.Positions;
using API.Domain.Common.Models;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Personnel.Positions.Queries.GetAll
{
    public record GetAllPositionsQuery
        : PaginationQuery, IRequest<PaginationResult<IEnumerable<PositionResponse>>>
    {
        public Guid? DepartmentId { get; init; }
    }
}

