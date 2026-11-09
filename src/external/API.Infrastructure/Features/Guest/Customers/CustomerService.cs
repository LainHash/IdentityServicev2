using API.Application.Features.Guest;
using API.Application.Features.Guest.Queries.GetAll;
using API.Application.Features.Guest.Queries.GetById;
using API.Application.Features.Guest.Queries.GetByUserId;
using API.Contract.DTOs.Guest.Customers;
using API.Domain.Common.Models.Messages;
using API.Domain.Common.Models.Results;
using API.Domain.Features.Guest.Customers;
using AutoMapper;
using System.Net;

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

        public async Task<Result<CustomerDetailResponse>> GetByIdAsync(
            GetCustomerByIdSpecification specification, 
            CancellationToken cancellationToken = default)
        {
            var customer = await _customerRepository
                .FindAsync(specification, cancellationToken);
            if(customer is null)
            {
                return Result<CustomerDetailResponse>
                    .Fail(Error.NotFound("Customer"), HttpStatusCode.NotFound);
            }

            var response = _mapper.Map<CustomerDetailResponse>(customer);
            return Result<CustomerDetailResponse>
                .Succeed(response, Success.Retrieved("Customer"));
        }

        public async Task<Result<CustomerDetailResponse>> GetByUserIdAsync(
            GetCustomerByUserIdSpecification specification, 
            CancellationToken cancellationToken = default)
        {
            var customer = await _customerRepository
                .FindAsync(specification, cancellationToken);
            if(customer is null)
            {
                return Result<CustomerDetailResponse>
                    .Fail(Error.NotFound("Customer"), HttpStatusCode.NotFound);
            }

            var response = _mapper.Map<CustomerDetailResponse>(customer);
            return Result<CustomerDetailResponse>
                .Succeed(response, Success.Retrieved("Customer"));
        }
    }
}
