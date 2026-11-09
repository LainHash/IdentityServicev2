using Microsoft.EntityFrameworkCore;
using API.Infrastructure.Context;
using API.Domain.Features.Guest.Customers;
using API.Infrastructure.Common.Repositories;

namespace API.Infrastructure.Features.Guest.Customers
{
    internal class CustomerRepository(AppDbContext context)
        : Repository<Customer>(context), ICustomerRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<Customer?> FindByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<Customer?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(x => x.PublicId == id, cancellationToken);
        }

        public async Task<Customer?> FindByUserIdAsync(long userId, CancellationToken cancellationToken = default)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        }

        public async Task<Customer?> FindByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.Customers
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.User.PublicId == userId, cancellationToken);
        }

        public async Task<Customer?> FindFullPersonalProfileAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.Customers
                .Include(x => x.User)
                    .ThenInclude(x => x.PersonalProfile)
                .FirstOrDefaultAsync(x => x.User.PublicId == userId, cancellationToken);
        }

        public async Task<Customer?> FindWithDetailsByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Customers
                .Include(x => x.User)
                    .ThenInclude(u => u.PersonalProfile)
                .Include(x => x.LoyaltyTransactions.OrderByDescending(t => t.CreatedAt).Take(20))
                .FirstOrDefaultAsync(x => x.PublicId == id, cancellationToken);
        }

        public async Task<Customer?> FindWithDetailsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.Customers
                .Include(x => x.User)
                    .ThenInclude(u => u.PersonalProfile)
                .Include(x => x.LoyaltyTransactions.OrderByDescending(t => t.CreatedAt).Take(20))
                .FirstOrDefaultAsync(x => x.User.PublicId == userId, cancellationToken);
        }
    }
}
