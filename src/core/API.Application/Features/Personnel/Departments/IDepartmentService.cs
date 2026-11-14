using API.Application.Features.Personnel.Departments.Commands.Create;
using API.Application.Features.Personnel.Departments.Commands.Delete;
using API.Application.Features.Personnel.Departments.Commands.Restore;
using API.Application.Features.Personnel.Departments.Commands.Update;
using API.Application.Features.Personnel.Departments.Queries.GetAll;
using API.Application.Features.Personnel.Departments.Queries.GetById;
using API.Contract.DTOs.Personnel.Departments;
using API.Domain.Common.Models.Results;

namespace API.Application.Features.Personnel.Departments
{
    public interface IDepartmentService
    {
        Task<PaginationResult<IEnumerable<DepartmentResponse>>> GetAllAsync(
            GetAllDepartmentsSpecification specification,
            CancellationToken cancellationToken = default);

        Task<Result<DepartmentDetailResponse>> GetByIdAsync(
            GetDepartmentByIdSpecification specification,
            CancellationToken cancellationToken = default);

        Task<Result<DepartmentResponse>> CreateAsync(
            CreateDepartmentCommand command,
            CancellationToken cancellationToken = default);

        Task<Result<DepartmentResponse>> UpdateAsync(
            UpdateDepartmentCommand command,
            CancellationToken cancellationToken = default);

        Task<Result> DeleteAsync(
            DeleteDepartmentCommand command,
            CancellationToken cancellationToken = default);

        Task<Result> RestoreAsync(
            RestoreDepartmentCommand command,
            CancellationToken cancellationToken = default);
    }
}

