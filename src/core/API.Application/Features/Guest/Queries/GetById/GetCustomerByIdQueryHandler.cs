using API.Contract.DTOs.Guest.Customers;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Guest.Queries.GetById
{
    internal class GetCustomerByIdQueryHandler(ICustomerService customerService)
                : IRequestHandler<GetCustomerByIdQuery, Result<CustomerDetailResponse>>
    {
        private readonly ICustomerService _customerService = customerService;

        public async Task<Result<CustomerDetailResponse>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var specification = new GetCustomerByIdSpecification(request);
            return await _customerService.GetByIdAsync(specification, cancellationToken);
        }
    }
}
