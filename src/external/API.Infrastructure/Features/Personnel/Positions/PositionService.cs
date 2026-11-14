using API.Application.Features.Business;
using API.Application.Features.Personnel.Positions;
using API.Application.Features.Personnel.Positions.Commands.Create;
using API.Application.Features.Personnel.Positions.Commands.Delete;
using API.Application.Features.Personnel.Positions.Commands.Restore;
using API.Application.Features.Personnel.Positions.Commands.Update;
using API.Application.Features.Personnel.Positions.Queries.GetAll;
using API.Application.Features.Personnel.Positions.Queries.GetById;
using API.Contract.DTOs.Personnel.Positions;
using API.Domain.Common.Helpers;
using API.Domain.Common.Models.Messages;
using API.Domain.Common.Models.Results;
using API.Domain.Features.Personnel.Departments;
using API.Domain.Features.Personnel.Positions;
using AutoMapper;
using System.Net;

namespace API.Infrastructure.Features.Personnel.Positions
{
    internal class PositionService : IPositionService
    {
        private readonly IPositionRepository _positionRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PositionService(
            IPositionRepository positionRepository,
            IDepartmentRepository departmentRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _positionRepository = positionRepository;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginationResult<IEnumerable<PositionResponse>>> GetAllAsync(
            GetAllPositionsSpecification specification,
            CancellationToken cancellationToken = default)
        {
            var positions = await _positionRepository
                .ToListAsync(specification, cancellationToken);

            var totalItems = await _positionRepository
                .CountAsync(specification, cancellationToken);

            var response = _mapper.Map<IEnumerable<PositionResponse>>(positions);
            return PaginationResult<IEnumerable<PositionResponse>>
                .Succeed(response, Success.Retrieved("Positions"), totalItems, specification.Skip, specification.Take);
        }

        public async Task<Result<PositionDetailResponse>> GetByIdAsync(
            GetPositionByIdSpecification specification,
            CancellationToken cancellationToken = default)
        {
            var position = await _positionRepository
                .FindAsync(specification, cancellationToken);

            if (position is null)
            {
                return Result<PositionDetailResponse>
                    .Fail(Error.NotFound("Position"), HttpStatusCode.NotFound);
            }

            var response = _mapper.Map<PositionDetailResponse>(position);
            return Result<PositionDetailResponse>
                .Succeed(response, Success.Retrieved("Position"));
        }

        public async Task<Result<PositionResponse>> CreateAsync(
            CreatePositionCommand command,
            CancellationToken cancellationToken = default)
        {
            var department = await _departmentRepository
                .FindByIdAsync(command.Body.DepartmentId, cancellationToken);

            if (department is null || department.IsDeleted)
            {
                return Result<PositionResponse>
                    .Fail(Error.NotFound("Department"), HttpStatusCode.NotFound);
            }

            var isExistingName = await _positionRepository
                .IsExistingNameAsync(null, command.Body.Name, cancellationToken);
            if (isExistingName)
            {
                return Result<PositionResponse>
                    .Fail(Error.ExistedName("Position"), HttpStatusCode.Conflict);
            }

            var code = string.IsNullOrWhiteSpace(command.Body.Code)
                ? CodeGenerator.Generate()
                : command.Body.Code.Trim();

            var isExistingCode = await _positionRepository
                .IsExistingCodeAsync(null, code, cancellationToken);
            if (isExistingCode)
            {
                return Result<PositionResponse>
                    .Fail("Position with this code already exists.", HttpStatusCode.Conflict);
            }

            var position = new Position(
                command.Body.Name.Trim(),
                code,
                department.Id,
                command.Body.Description?.Trim());

            _positionRepository.Add(position);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var createdPosition = await _positionRepository
                .FindByIdAsync(position.PublicId, cancellationToken);

            var response = _mapper.Map<PositionResponse>(createdPosition ?? position);
            return Result<PositionResponse>
                .Succeed(response, Success.Created("Position"), HttpStatusCode.Created);
        }

        public async Task<Result<PositionResponse>> UpdateAsync(
            UpdatePositionCommand command,
            CancellationToken cancellationToken = default)
        {
            var position = await _positionRepository
                .FindByIdAsync(command.Id, cancellationToken);

            if (position is null || position.IsDeleted)
            {
                return Result<PositionResponse>
                    .Fail(Error.NotFound("Position"), HttpStatusCode.NotFound);
            }

            if (position.Department.PublicId != command.Body.DepartmentId)
            {
                var department = await _departmentRepository
                    .FindByIdAsync(command.Body.DepartmentId, cancellationToken);

                if (department is null || department.IsDeleted)
                {
                    return Result<PositionResponse>
                        .Fail(Error.NotFound("Department"), HttpStatusCode.NotFound);
                }

                position.SetDepartment(department.Id);
            }

            var isExistingName = await _positionRepository
                .IsExistingNameAsync(position.PublicId, command.Body.Name, cancellationToken);
            if (isExistingName)
            {
                return Result<PositionResponse>
                    .Fail(Error.ExistedName("Position"), HttpStatusCode.Conflict);
            }

            var code = string.IsNullOrWhiteSpace(command.Body.Code)
                ? position.Code
                : command.Body.Code.Trim();

            var isExistingCode = await _positionRepository
                .IsExistingCodeAsync(position.PublicId, code, cancellationToken);
            if (isExistingCode)
            {
                return Result<PositionResponse>
                    .Fail("Position with this code already exists.", HttpStatusCode.Conflict);
            }

            position.Update(command.Body.Name.Trim(), code, command.Body.Description?.Trim());

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var updatedPosition = await _positionRepository
                .FindByIdAsync(position.PublicId, cancellationToken);

            var response = _mapper.Map<PositionResponse>(updatedPosition ?? position);
            return Result<PositionResponse>
                .Succeed(response, Success.Updated("Position"));
        }

        public async Task<Result> DeleteAsync(
            DeletePositionCommand command,
            CancellationToken cancellationToken = default)
        {
            var position = await _positionRepository
                .FindByIdAsync(command.Id, cancellationToken);

            if (position is null)
            {
                return Result.Fail(Error.NotFound("Position"), HttpStatusCode.NotFound);
            }

            if (position.IsDeleted)
            {
                return Result.Fail(Error.AlreadyDeleted("Position"), HttpStatusCode.Conflict);
            }

            var hasActiveEmployees = await _positionRepository
                .HasActiveEmployeesAsync(position.Id, cancellationToken);
            if (hasActiveEmployees)
            {
                return Result.Fail(
                    "Cannot delete position because employees are still assigned to it.",
                    HttpStatusCode.Conflict);
            }

            position.SoftDelete();

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Succeed(Success.Deleted("Position"));
        }

        public async Task<Result> RestoreAsync(
            RestorePositionCommand command,
            CancellationToken cancellationToken = default)
        {
            var position = await _positionRepository
                .FindByIdAsync(command.Id, cancellationToken);

            if (position is null)
            {
                return Result.Fail(Error.NotFound("Position"), HttpStatusCode.NotFound);
            }

            if (!position.IsDeleted)
            {
                return Result.Fail(Error.NotYetDeleted("Position"), HttpStatusCode.Conflict);
            }

            position.Restore();

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Succeed(Success.Restored("Position"));
        }
    }
}

