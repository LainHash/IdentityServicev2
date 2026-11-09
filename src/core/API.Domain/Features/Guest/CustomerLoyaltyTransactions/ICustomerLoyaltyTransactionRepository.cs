using API.Domain.Common.Repositories;

namespace API.Domain.Features.Guest.CustomerLoyaltyTransactions
{
    public interface ICustomerLoyaltyTransactionRepository : IRepository<CustomerLoyaltyTransaction>
    {
        Task<List<CustomerLoyaltyTransaction>> GetRecentByCustomerIdAsync(long customerId, int take = 10, CancellationToken cancellationToken = default);
    }
}
