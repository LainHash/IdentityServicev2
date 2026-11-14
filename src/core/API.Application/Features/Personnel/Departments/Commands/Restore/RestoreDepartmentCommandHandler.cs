using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Personnel.Departments.Commands.Restore
{
    internal class RestoreDepartmentCommandHandler(IDepartmentService departmentService)
        : IRequestHandler<RestoreDepartmentCommand, Result>
    {
        private readonly IDepartmentService _departmentService = departmentService;

        public async Task<Result> Handle(
            RestoreDepartmentCommand request,
            CancellationToken cancellationToken)
        {
            return await _departmentService.RestoreAsync(request, cancellationToken);
        }
    }
}

