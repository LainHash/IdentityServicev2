using Microsoft.EntityFrameworkCore;
using API.Domain.Features.Identity.Roles;
using API.Infrastructure.Common.Repositories;
using API.Infrastructure.Contexts.Data;

namespace API.Infrastructure.Features.Identity.Roles
{
    internal class RoleRepository(AppDbContext context)
        : Repository<Role>(context), IRoleRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<Role?> FindByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<Role?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(x => x.PublicId == id, cancellationToken);
        }

        public async Task<Role?> FindByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _context.Roles.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
        }

        public async Task<bool> IsExistingNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _context.Roles.AnyAsync(x => x.Name == name, cancellationToken);
        }

        public async Task<bool> IsExistingNameAsync(
            Guid? excludePublicId,
            string name,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Roles.AsQueryable();
            if (excludePublicId.HasValue)
            {
                query = query.Where(x => x.PublicId != excludePublicId.Value);
            }
            return await query.AnyAsync(x => EF.Functions.ILike(x.Name, name.Trim()), cancellationToken);
        }
    }
}
