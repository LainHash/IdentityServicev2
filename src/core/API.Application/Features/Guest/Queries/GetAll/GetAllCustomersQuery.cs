using API.Contract.DTOs.Guest.Customers;
using API.Domain.Common.Models;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Guest.Queries.GetAll
{
    public record GetAllCustomersQuery()
        : PaginationQuery, IRequest<PaginationResult<IEnumerable<CustomerResponse>>>
    {
    }
}
