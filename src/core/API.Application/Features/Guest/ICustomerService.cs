using API.Application.Features.Guest.Queries.GetAll;
using API.Contract.DTOs.Guest.Customers;
using API.Domain.Common.Models.Results;

namespace API.Application.Features.Guest
{
    public interface ICustomerService
    {
        Task<PaginationResult<IEnumerable<CustomerResponse>>> GetAllAsync(
            GetAllCustomersSpecification specification,
            CancellationToken cancellationToken = default);
    }
}
