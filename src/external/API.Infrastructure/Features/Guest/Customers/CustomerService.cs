using API.Application.Features.Guest;
using API.Application.Features.Guest.Queries.GetAll;
using API.Contract.DTOs.Guest.Customers;
using API.Domain.Common.Models.Messages;
using API.Domain.Common.Models.Results;
using API.Domain.Features.Guest.Customers;
using AutoMapper;

namespace API.Infrastructure.Features.Guest.Customers
{
    internal class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        private readonly IMapper _mapper;

        public CustomerService(
            ICustomerRepository customerRepository,
            IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<PaginationResult<IEnumerable<CustomerResponse>>> GetAllAsync(
            GetAllCustomersSpecification specification, 
            CancellationToken cancellationToken = default)
        {
            var totalItems = await _customerRepository
                .CountAsync(specification, cancellationToken);

            var customers = await _customerRepository
                .ToListAsync(specification, cancellationToken);

            var response = _mapper.Map<IEnumerable<CustomerResponse>>(customers);
            return PaginationResult<IEnumerable<CustomerResponse>>
                .Succeed(response, Success.Retrieved("Customers"), totalItems, specification.Skip, specification.Take);
        }
    }
}
