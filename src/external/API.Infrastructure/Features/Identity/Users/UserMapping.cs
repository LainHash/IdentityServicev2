using API.Contract.DTOs;
using API.Contract.DTOs.Authentication;
using API.Contract.DTOs.Identity.Users;
using API.Domain.Common.Entities;
using API.Domain.Features.Identity.Users;
using AutoMapper;

namespace API.Infrastructure.Features.Identity.Users
{
    internal class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<User, UserResponse>()
                .ForMember(dest => dest.RoleName, 
                    opt => opt.MapFrom(src => src.Role.Name))
                .IncludeBase<SoftDeletableEntity, SoftDeletableDTOResponse>();

            CreateMap<RegisterRequest, User>();

            CreateMap<User, UserMinimalResponse>()
                .IncludeBase<SoftDeletableEntity, SoftDeletableDTOResponse>();
        }
    }
}
