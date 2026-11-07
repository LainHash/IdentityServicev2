using API.Contract.DTOs;
using API.Contract.DTOs.Identity.Roles;
using API.Domain.Common.Entities;
using API.Domain.Features.Identity.Roles;
using AutoMapper;

namespace API.Infrastructure.Features.Identity.Roles
{
    internal class RoleMapping : Profile
    {
        public RoleMapping()
        {
            CreateMap<Role, RoleResponse>()
                .ForMember(dest => dest.UserCount,
                    opt => opt.MapFrom(src => src.Users.Count))
                .IncludeBase<SoftDeletableEntity, SoftDeletableDTOResponse>();

            CreateMap<Role, RoleDetailResponse>()
                .IncludeBase<Role, RoleResponse>();
        }
    }
}
