using API.Contract.DTOs;
using API.Contract.DTOs.Personnel.Departments;
using API.Contract.ImportDataRows.Identity;
using API.Domain.Common.Entities;
using API.Domain.Features.Personnel.Departments;
using AutoMapper;

namespace API.Infrastructure.Features.Personnel.Departments
{
    internal class DepartmentMapping : Profile
    {
        public DepartmentMapping()
        {
            CreateMap<Department, DepartmentResponse>()
                .IncludeBase<SoftDeletableEntity, SoftDeletableResponse>();

            CreateMap<Department, DepartmentDetailResponse>()
                .IncludeBase<Department, DepartmentResponse>();

            CreateMap<CreateDepartmentRequest, Department>();

            CreateMap<UpdateDepartmentRequest, Department>();

            CreateMap<DepartmentRow, Department>();
        }
    }
}
