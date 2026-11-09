using API.Domain.Features.Guest.CustomerLoyaltyTransactions;
using API.Infrastructure.Common.Repositories;
using API.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure.Features.Guest.CustomerLoyaltyTransactions
{
    internal class CustomerLoyaltyTransactionRepository(AppDbContext context)
        : Repository<CustomerLoyaltyTransaction>(context), ICustomerLoyaltyTransactionRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<List<CustomerLoyaltyTransaction>> GetRecentByCustomerIdAsync(
            long customerId,
            int take = 10,
            CancellationToken cancellationToken = default)
        {
            return await _context.CustomerLoyaltyTransactions
                .Where(t => t.CustomerId == customerId)
                .OrderByDescending(t => t.CreatedAt)
                .Take(take)
                .ToListAsync(cancellationToken);
        }
    }
}
