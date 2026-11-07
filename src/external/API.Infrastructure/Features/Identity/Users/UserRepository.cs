using API.Domain.Features.Identity.Users;
using API.Infrastructure.Common.Repositories;
using API.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;

namespace API.Infrastructure.Features.Identity.Users
{
    internal class UserRepository(AppDbContext context) 
        : Repository<User>(context), IUserRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        }

        public async Task<User?> FindByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<User?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.PublicId == id, cancellationToken);
        }

        public Task<User?> FindByIdentifierAsync(
            string identifier,
            CancellationToken cancellationToken = default)
        {
            identifier = identifier.Trim();

            if (MailAddress.TryCreate(identifier, out _))
            {
                return _context.Users
                    .FirstOrDefaultAsync(
                        x => x.Email == identifier,
                        cancellationToken);
            }

            return _context.Users
                .FirstOrDefaultAsync(
                    x => x.UserName == identifier,
                    cancellationToken);
        }

        public async Task<User?> FindByIdWithRoleAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.PublicId == id, cancellationToken);
        }

        public async Task<bool> IsExistingEmailAsync(Guid? excludePublicId, string email, CancellationToken cancellationToken = default)
        {
            var query = _context.Users.AsQueryable();
            if (excludePublicId.HasValue)
            {
                query = query.Where(x => x.PublicId != excludePublicId.Value);
            }
            return await query.AnyAsync(x => EF.Functions.ILike(x.Email, email), cancellationToken);
        }

        public async Task<bool> IsExistingUserNameAsync(Guid? excludePublicId, string userName, CancellationToken cancellationToken = default)
        {
            var query = _context.Users.AsQueryable();
            if (excludePublicId.HasValue)
            {
                query = query.Where(x => x.PublicId != excludePublicId.Value);
            }
            return await query.AnyAsync(x => EF.Functions.ILike(x.UserName, userName), cancellationToken);
        }
    }
}
