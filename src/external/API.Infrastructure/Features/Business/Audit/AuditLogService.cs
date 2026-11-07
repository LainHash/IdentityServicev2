using API.Application.Features.Business.Audit;
using API.Application.Features.Business.Audit.Queries.GetAll;
using API.Contract.DTOs.Business.AuditLogs;
using API.Domain.Common.Models.Messages;
using API.Domain.Common.Models.Results;
using API.Domain.Features.Business.Audit;
using AutoMapper;

namespace API.Infrastructure.Features.Business.Audit
{
    internal class AuditLogService : IAuditLogService
    {
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IMapper _mapper;

        public AuditLogService(
            IAuditLogRepository auditLogRepository, 
            IMapper mapper)
        {
            _auditLogRepository = auditLogRepository;
            _mapper = mapper;
        }

        public async Task<PaginationResult<IEnumerable<AuditLogResponse>>> GetAllAsync(GetAllAuditLogsSpecification specification, CancellationToken cancellationToken = default)
        {
            var totalItems = await _auditLogRepository.CountAsync(specification, cancellationToken);

            var auditLogs = await _auditLogRepository.ToListAsync(specification, cancellationToken);

            var response = _mapper.Map<IEnumerable<AuditLogResponse>>(auditLogs);
            return PaginationResult<IEnumerable<AuditLogResponse>>.Succeed(
                response,
                Success.Retrieved("Audit Logs"),
                totalItems,
                specification.Skip,
                specification.Take);
        }

        public async Task PurgeOldLogsAsync(int retentionDays, CancellationToken cancellationToken = default)
        {
            var cutoff = DateTime.UtcNow.AddDays(-retentionDays);
            await _auditLogRepository.PurgeOldLogsAsync(cutoff, cancellationToken);
        }
    }
}
