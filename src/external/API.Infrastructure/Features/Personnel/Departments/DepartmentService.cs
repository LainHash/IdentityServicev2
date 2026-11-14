using API.Application.Features.Business;
using API.Application.Features.Personnel.Departments;
using API.Application.Features.Personnel.Departments.Commands.Create;
using API.Application.Features.Personnel.Departments.Commands.Delete;
using API.Application.Features.Personnel.Departments.Commands.Restore;
using API.Application.Features.Personnel.Departments.Commands.Update;
using API.Application.Features.Personnel.Departments.Queries.GetAll;
using API.Application.Features.Personnel.Departments.Queries.GetById;
using API.Contract.DTOs.Personnel.Departments;
using API.Domain.Common.Helpers;
using API.Domain.Common.Models.Messages;
using API.Domain.Common.Models.Results;
using API.Domain.Features.Personnel.Departments;
using AutoMapper;
using System.Net;

namespace API.Infrastructure.Features.Personnel.Departments
{
    internal class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(
            IDepartmentRepository departmentRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginationResult<IEnumerable<DepartmentResponse>>> GetAllAsync(
            GetAllDepartmentsSpecification specification,
            CancellationToken cancellationToken = default)
        {
            var departments = await _departmentRepository
                .ToListAsync(specification, cancellationToken);

            var totalItems = await _departmentRepository
                .CountAsync(specification, cancellationToken);

            var response = _mapper.Map<IEnumerable<DepartmentResponse>>(departments);
            return PaginationResult<IEnumerable<DepartmentResponse>>
                .Succeed(response, Success.Retrieved("Departments"), totalItems, specification.Skip, specification.Take);
        }

        public async Task<Result<DepartmentDetailResponse>> GetByIdAsync(
            GetDepartmentByIdSpecification specification,
            CancellationToken cancellationToken = default)
        {
            var department = await _departmentRepository
                .FindAsync(specification, cancellationToken);

            if (department is null)
            {
                return Result<DepartmentDetailResponse>
                    .Fail(Error.NotFound("Department"), HttpStatusCode.NotFound);
            }

            var response = _mapper.Map<DepartmentDetailResponse>(department);
            return Result<DepartmentDetailResponse>
                .Succeed(response, Success.Retrieved("Department"));
        }

        public async Task<Result<DepartmentResponse>> CreateAsync(
            CreateDepartmentCommand command,
            CancellationToken cancellationToken = default)
        {
            var isExistingName = await _departmentRepository
                .IsExistingNameAsync(null, command.Body.Name, cancellationToken);
            if (isExistingName)
            {
                return Result<DepartmentResponse>
                    .Fail(Error.ExistedName("Department"), HttpStatusCode.Conflict);
            }

            var code = string.IsNullOrWhiteSpace(command.Body.Code)
                ? CodeGenerator.Generate()
                : command.Body.Code.Trim();

            var isExistingCode = await _departmentRepository
                .IsExistingCodeAsync(null, code, cancellationToken);
            if (isExistingCode)
            {
                return Result<DepartmentResponse>
                    .Fail("Department with this code already exists.", HttpStatusCode.Conflict);
            }

            var department = new Department(command.Body.Name.Trim(), code, command.Body.Description?.Trim());
            _departmentRepository.Add(department);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<DepartmentResponse>(department);
            return Result<DepartmentResponse>
                .Succeed(response, Success.Created("Department"), HttpStatusCode.Created);
        }

        public async Task<Result<DepartmentResponse>> UpdateAsync(
            UpdateDepartmentCommand command,
            CancellationToken cancellationToken = default)
        {
            var department = await _departmentRepository
                .FindByIdAsync(command.Id, cancellationToken);

            if (department is null || department.IsDeleted)
            {
                return Result<DepartmentResponse>
                    .Fail(Error.NotFound("Department"), HttpStatusCode.NotFound);
            }

            var isExistingName = await _departmentRepository
                .IsExistingNameAsync(department.PublicId, command.Body.Name, cancellationToken);
            if (isExistingName)
            {
                return Result<DepartmentResponse>
                    .Fail(Error.ExistedName("Department"), HttpStatusCode.Conflict);
            }

            var code = string.IsNullOrWhiteSpace(command.Body.Code)
                ? department.Code
                : command.Body.Code.Trim();

            var isExistingCode = await _departmentRepository
                .IsExistingCodeAsync(department.PublicId, code, cancellationToken);
            if (isExistingCode)
            {
                return Result<DepartmentResponse>
                    .Fail("Department with this code already exists.", HttpStatusCode.Conflict);
            }

            department.Update(command.Body.Name.Trim(), code, command.Body.Description?.Trim());

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<DepartmentResponse>(department);
            return Result<DepartmentResponse>
                .Succeed(response, Success.Updated("Department"));
        }

        public async Task<Result> DeleteAsync(
            DeleteDepartmentCommand command,
            CancellationToken cancellationToken = default)
        {
            var department = await _departmentRepository
                .FindByIdAsync(command.Id, cancellationToken);

            if (department is null)
            {
                return Result.Fail(Error.NotFound("Department"), HttpStatusCode.NotFound);
            }

            if (department.IsDeleted)
            {
                return Result.Fail(Error.AlreadyDeleted("Department"), HttpStatusCode.Conflict);
            }

            var hasActivePositions = await _departmentRepository
                .HasActivePositionsAsync(department.Id, cancellationToken);
            if (hasActivePositions)
            {
                return Result.Fail(
                    "Cannot delete department because it still has active positions.",
                    HttpStatusCode.Conflict);
            }

            department.SoftDelete();

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Succeed(Success.Deleted("Department"));
        }

        public async Task<Result> RestoreAsync(
            RestoreDepartmentCommand command,
            CancellationToken cancellationToken = default)
        {
            var department = await _departmentRepository
                .FindByIdAsync(command.Id, cancellationToken);

            if (department is null)
            {
                return Result.Fail(Error.NotFound("Department"), HttpStatusCode.NotFound);
            }

            if (!department.IsDeleted)
            {
                return Result.Fail(Error.NotYetDeleted("Department"), HttpStatusCode.Conflict);
            }

            department.Restore();

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Succeed(Success.Restored("Department"));
        }
    }
}

