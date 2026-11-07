using API.Contract.DTOs.Business.AuditLogs;
using API.Domain.Common.Enums;
using API.Domain.Common.Models;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Business.Audit.Queries.GetAll
{
    public record GetAllAuditLogsQuery(
        string? EntityName = null,
        Guid? UserPublicId = null,
        AuditAction[]? Actions = null)
        : PaginationQuery, IRequest<PaginationResult<IEnumerable<AuditLogResponse>>>
    {

    }
}
