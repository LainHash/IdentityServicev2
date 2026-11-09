using API.Contract.DTOs.Guest.Customers;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Guest.Queries.GetByUserId
{
    internal class GetCustomerByUserIdQueryHandler(ICustomerService customerService)
        : IRequestHandler<GetCustomerByUserIdQuery, Result<CustomerDetailResponse>>
    {
        private readonly ICustomerService _customerService = customerService;

        public async Task<Result<CustomerDetailResponse>> Handle(GetCustomerByUserIdQuery request, CancellationToken cancellationToken)
        {
            var specification = new GetCustomerByUserIdSpecification(request);
            return await _customerService.GetByUserIdAsync(specification, cancellationToken);
        }
    }
}

