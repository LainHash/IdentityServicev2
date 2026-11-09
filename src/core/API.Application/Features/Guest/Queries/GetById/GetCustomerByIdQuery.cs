using API.Contract.DTOs.Guest.Customers;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Guest.Queries.GetById
{
    public record GetCustomerByIdQuery(Guid Id)
        : IRequest<Result<CustomerDetailResponse>>
    {
    }
}
