using API.Contract.DTOs.Guest.Customers;
using API.Domain.Common.Models;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Guest.Queries.GetAll
{
    internal class GetAllCustomersQueryHandler(ICustomerService customerService)
                : IRequestHandler<GetAllCustomersQuery, PaginationResult<IEnumerable<CustomerResponse>>>
    {
        private readonly ICustomerService _customerService = customerService;

        public async Task<PaginationResult<IEnumerable<CustomerResponse>>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            var specification = new GetAllCustomersSpecification(request);
            return await _customerService.GetAllAsync(specification, cancellationToken);
        }
    }
}
