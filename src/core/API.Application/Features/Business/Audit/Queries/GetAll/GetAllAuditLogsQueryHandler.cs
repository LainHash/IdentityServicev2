using API.Contract.DTOs.Business.AuditLogs;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Business.Audit.Queries.GetAll
{
    internal class GetAllAuditLogsQueryHandler(IAuditLogService auditLogService)
                : IRequestHandler<GetAllAuditLogsQuery, PaginationResult<IEnumerable<AuditLogResponse>>>
    {
        private readonly IAuditLogService _auditLogService = auditLogService;

        public async Task<PaginationResult<IEnumerable<AuditLogResponse>>> Handle(GetAllAuditLogsQuery request, CancellationToken cancellationToken)
        {
            var specification = new GetAllAuditLogsSpecification(request);
            return await _auditLogService.GetAllAsync(specification, cancellationToken);
        }
    }
}
