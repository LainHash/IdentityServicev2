using API.Contract.DTOs.Personnel.Departments;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Personnel.Departments.Commands.Create
{
    internal class CreateDepartmentCommandHandler(IDepartmentService departmentService)
        : IRequestHandler<CreateDepartmentCommand, Result<DepartmentResponse>>
    {
        private readonly IDepartmentService _departmentService = departmentService;

        public async Task<Result<DepartmentResponse>> Handle(
            CreateDepartmentCommand request,
            CancellationToken cancellationToken)
        {
            return await _departmentService.CreateAsync(request, cancellationToken);
        }
    }
}

