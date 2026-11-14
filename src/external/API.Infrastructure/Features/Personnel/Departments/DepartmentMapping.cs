using API.Contract.DTOs.Personnel.Departments;
using API.Contract.ImportDataRows.Identity;
using API.Domain.Features.Personnel.Departments;
using AutoMapper;

namespace API.Infrastructure.Features.Personnel.Departments
{
    internal class DepartmentMapping : Profile
    {
        public DepartmentMapping()
        {
            CreateMap<Department, DepartmentResponse>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.PublicId));

            CreateMap<Department, DepartmentDetailResponse>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.PublicId))
                .ForMember(dest => dest.Positions,
                    opt => opt.MapFrom(src => src.Positions));

            CreateMap<CreateDepartmentRequest, Department>();

            CreateMap<UpdateDepartmentRequest, Department>();

            CreateMap<DepartmentRow, Department>();
        }
    }
}
