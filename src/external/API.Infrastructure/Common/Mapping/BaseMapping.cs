using API.Contract.DTOs;
using API.Domain.Common.Entities;
using AutoMapper;

namespace API.Infrastructure.Common.Mapping
{
    internal class BaseMapping : Profile
    {
        public BaseMapping()
        {
            CreateMap<Entity, BaseDTOResponse>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.PublicId));

            CreateMap<AuditableEntity, AuditableResponse>()
                .IncludeBase<Entity, BaseDTOResponse>();

            CreateMap<SoftDeletableEntity, SoftDeletableResponse>()
                .IncludeBase<AuditableEntity, AuditableResponse>();
        }
    }
}
