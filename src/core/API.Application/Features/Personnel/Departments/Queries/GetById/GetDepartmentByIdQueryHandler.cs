using API.Contract.DTOs.Personnel.Departments;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Personnel.Departments.Queries.GetById
{
    internal class GetDepartmentByIdQueryHandler(IDepartmentService departmentService)
        : IRequestHandler<GetDepartmentByIdQuery, Result<DepartmentDetailResponse>>
    {
        private readonly IDepartmentService _departmentService = departmentService;

        public async Task<Result<DepartmentDetailResponse>> Handle(
            GetDepartmentByIdQuery request,
            CancellationToken cancellationToken)
        {
            var specification = new GetDepartmentByIdSpecification(request);
            return await _departmentService.GetByIdAsync(specification, cancellationToken);
        }
    }
}

