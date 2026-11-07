using API.Contract.DTOs;
using API.Contract.DTOs.Business.AuditLogs;
using API.Domain.Common.Entities;
using API.Domain.Features.Business.Audit;
using AutoMapper;

namespace API.Infrastructure.Features.Business.Audit
{
    internal class AuditLogMapping : Profile
    {
        public AuditLogMapping()
        {
            CreateMap<AuditLog, AuditLogResponse>()
                .IncludeBase<Entity, BaseDTOResponse>();
        }
    }
}
