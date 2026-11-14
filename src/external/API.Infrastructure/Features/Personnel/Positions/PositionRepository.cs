using API.Domain.Features.Personnel.Positions;
using API.Infrastructure.Common.Repositories;
using API.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure.Features.Personnel.Positions
{
    internal class PositionRepository(AppDbContext context)
        : Repository<Position>(context), IPositionRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<Position?> FindByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _context.Positions
                .Include(x => x.Department)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<Position?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Positions
                .Include(x => x.Department)
                .FirstOrDefaultAsync(x => x.PublicId == id, cancellationToken);
        }

        public async Task<bool> IsExistingNameAsync(Guid? excludePublicId, string name, CancellationToken cancellationToken = default)
        {
            var query = _context.Positions.AsQueryable();
            if (excludePublicId.HasValue)
            {
                query = query.Where(x => x.PublicId != excludePublicId.Value);
            }
            return await query.AnyAsync(x => EF.Functions.ILike(x.Name, name.Trim()), cancellationToken);
        }

        public async Task<bool> IsExistingCodeAsync(Guid? excludePublicId, string code, CancellationToken cancellationToken = default)
        {
            var query = _context.Positions.AsQueryable();
            if (excludePublicId.HasValue)
            {
                query = query.Where(x => x.PublicId != excludePublicId.Value);
            }
            return await query.AnyAsync(x => EF.Functions.ILike(x.Code, code.Trim()), cancellationToken);
        }

        public async Task<bool> HasActiveEmployeesAsync(long positionId, CancellationToken cancellationToken = default)
        {
            return await _context.Employees
                .AnyAsync(x => x.PositionId == positionId && !x.IsDeleted, cancellationToken);
        }

        public async Task<HashSet<string>> GetExistingNamesAsync(
            IEnumerable<string> names,
            CancellationToken cancellationToken = default)
        {
            var nameList = names
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .Select(n => n.ToLower())
                .Distinct()
                .ToList();

            if (nameList.Count == 0)
            {
                return new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            }

            var existingNames = await _context.Positions
                .Where(x => nameList.Contains(x.Name.ToLower()))
                .Select(x => x.Name)
                .ToListAsync(cancellationToken);

            return existingNames.ToHashSet(StringComparer.OrdinalIgnoreCase);
        }

        public async Task<HashSet<Guid>> GetExistingPublicIdsAsync(
            IEnumerable<Guid> publicIds,
            CancellationToken cancellationToken = default)
        {
            var idList = publicIds
                .Where(x => x != Guid.Empty)
                .Distinct()
                .ToList();

            if (idList.Count == 0)
            {
                return [];
            }

            var existingIds = await _context.Positions
                .Where(x => idList.Contains(x.PublicId))
                .Select(x => x.PublicId)
                .ToListAsync(cancellationToken);

            return existingIds.ToHashSet();
        }
    }
}
