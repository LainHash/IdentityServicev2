using API.Domain.Common.Repositories;

namespace API.Domain.Features.Guest.Customers
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer?> FindByUserIdAsync(long userId, CancellationToken cancellationToken = default);
        Task<Customer?> FindByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

        Task<Customer?> FindByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<Customer?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
