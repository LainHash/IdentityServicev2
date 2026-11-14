using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Personnel.Departments.Commands.Delete
{
    internal class DeleteDepartmentCommandHandler(IDepartmentService departmentService)
        : IRequestHandler<DeleteDepartmentCommand, Result>
    {
        private readonly IDepartmentService _departmentService = departmentService;

        public async Task<Result> Handle(
            DeleteDepartmentCommand request,
            CancellationToken cancellationToken)
        {
            return await _departmentService.DeleteAsync(request, cancellationToken);
        }
    }
}

