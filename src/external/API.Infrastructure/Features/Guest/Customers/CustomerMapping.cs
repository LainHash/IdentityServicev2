using API.Contract.DTOs;
using API.Contract.DTOs.Guest.Customers;
using API.Domain.Common.Entities;
using API.Domain.Features.Guest.Customers;
using AutoMapper;

namespace API.Infrastructure.Features.Guest.Customers
{
    internal class CustomerMapping : Profile
    {
        public CustomerMapping()
        {
            CreateMap<Customer, CustomerResponse>()
                .IncludeBase<AuditableEntity, AuditableResponse>();
        }
    }
}
