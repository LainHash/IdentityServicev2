using API.Contract.DTOs.Guest.Customers;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Guest.Queries.GetByUserId
{
    public record GetCustomerByUserIdQuery(Guid UserId)
        : IRequest<Result<CustomerDetailResponse>>
    {
    }
}

