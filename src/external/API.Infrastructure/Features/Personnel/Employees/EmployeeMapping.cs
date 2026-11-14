using API.Contract.DTOs;
using API.Contract.DTOs.Personnel.Employees;
using API.Domain.Common.Entities;
using API.Domain.Features.Personnel.Employees;
using AutoMapper;

namespace API.Infrastructure.Features.Personnel.Employees
{
    internal class EmployeeMapping : Profile
    {
        public EmployeeMapping()
        {
            CreateMap<Employee, EmployeeResponse>()
                .ForMember(dest => dest.PositionName, opt =>
                    opt.MapFrom(src => src.Position.Name))
                .ForMember(dest => dest.PositionCode, opt =>
                    opt.MapFrom(src => src.Position.Code))
                .IncludeBase<SoftDeletableEntity, SoftDeletableResponse>();

            CreateMap<Employee, EmployeeDetailResponse>()
                .IncludeBase<Employee, EmployeeResponse>();
        }
    }
}
