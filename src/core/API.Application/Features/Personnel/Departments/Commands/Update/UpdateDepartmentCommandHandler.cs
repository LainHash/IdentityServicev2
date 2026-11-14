using API.Contract.DTOs.Personnel.Departments;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Personnel.Departments.Commands.Update
{
    internal class UpdateDepartmentCommandHandler(IDepartmentService departmentService)
        : IRequestHandler<UpdateDepartmentCommand, Result<DepartmentResponse>>
    {
        private readonly IDepartmentService _departmentService = departmentService;

        public async Task<Result<DepartmentResponse>> Handle(
            UpdateDepartmentCommand request,
            CancellationToken cancellationToken)
        {
            return await _departmentService.UpdateAsync(request, cancellationToken);
        }
    }
}

