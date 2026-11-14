using API.Application.Features.Personnel.Positions.Commands.Create;
using API.Application.Features.Personnel.Positions.Commands.Delete;
using API.Application.Features.Personnel.Positions.Commands.Restore;
using API.Application.Features.Personnel.Positions.Commands.Update;
using API.Application.Features.Personnel.Positions.Queries.GetAll;
using API.Application.Features.Personnel.Positions.Queries.GetById;
using API.Contract.DTOs.Personnel.Positions;
using API.Domain.Common.Models.Results;

namespace API.Application.Features.Personnel.Positions
{
    public interface IPositionService
    {
        Task<PaginationResult<IEnumerable<PositionResponse>>> GetAllAsync(
            GetAllPositionsSpecification specification,
            CancellationToken cancellationToken = default);

        Task<Result<PositionDetailResponse>> GetByIdAsync(
            GetPositionByIdSpecification specification,
            CancellationToken cancellationToken = default);

        Task<Result<PositionResponse>> CreateAsync(
            CreatePositionCommand command,
            CancellationToken cancellationToken = default);

        Task<Result<PositionResponse>> UpdateAsync(
            UpdatePositionCommand command,
            CancellationToken cancellationToken = default);

        Task<Result> DeleteAsync(
            DeletePositionCommand command,
            CancellationToken cancellationToken = default);

        Task<Result> RestoreAsync(
            RestorePositionCommand command,
            CancellationToken cancellationToken = default);
    }
}

