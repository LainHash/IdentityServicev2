using API.Contract.DTOs.Personnel.Positions;
using API.Contract.ImportDataRows.Identity;
using API.Domain.Features.Personnel.Positions;
using AutoMapper;

namespace API.Infrastructure.Features.Personnel.Positions
{
    internal class PositionMapping : Profile
    {
        public PositionMapping()
        {
            CreateMap<Position, PositionResponse>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.PublicId))
                .ForMember(dest => dest.DepartmentId,
                    opt => opt.MapFrom(src => src.Department.PublicId))
                .ForMember(dest => dest.DepartmentName,
                    opt => opt.MapFrom(src => src.Department.Name))
                .ForMember(dest => dest.DepartmentCode,
                    opt => opt.MapFrom(src => src.Department.Code));

            CreateMap<Position, PositionDetailResponse>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.PublicId))
                .ForMember(dest => dest.DepartmentId,
                    opt => opt.MapFrom(src => src.Department.PublicId))
                .ForMember(dest => dest.DepartmentName,
                    opt => opt.MapFrom(src => src.Department.Name))
                .ForMember(dest => dest.DepartmentCode,
                    opt => opt.MapFrom(src => src.Department.Code));

            CreateMap<CreatePositionRequest, Position>();

            CreateMap<UpdatePositionRequest, Position>();

            CreateMap<PositionRow, Position>();
        }
    }
}
