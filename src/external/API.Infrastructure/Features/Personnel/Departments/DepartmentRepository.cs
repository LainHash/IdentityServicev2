using API.Domain.Features.Personnel.Departments;
using API.Infrastructure.Common.Repositories;
using API.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure.Features.Personnel.Departments
{
    internal class DepartmentRepository(AppDbContext context)
        : Repository<Department>(context), IDepartmentRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<Department?> FindByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _context.Departments
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<Department?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Departments
                .FirstOrDefaultAsync(x => x.PublicId == id, cancellationToken);
        }

        public async Task<bool> IsExistingNameAsync(Guid? excludePublicId, string name, CancellationToken cancellationToken = default)
        {
            var query = _context.Departments.AsQueryable();
            if (excludePublicId.HasValue)
            {
                query = query.Where(x => x.PublicId != excludePublicId.Value);
            }
            return await query.AnyAsync(x => EF.Functions.ILike(x.Name, name.Trim()), cancellationToken);
        }

        public async Task<bool> IsExistingCodeAsync(Guid? excludePublicId, string code, CancellationToken cancellationToken = default)
        {
            var query = _context.Departments.AsQueryable();
            if (excludePublicId.HasValue)
            {
                query = query.Where(x => x.PublicId != excludePublicId.Value);
            }
            return await query.AnyAsync(x => EF.Functions.ILike(x.Code, code.Trim()), cancellationToken);
        }

        public async Task<bool> HasActivePositionsAsync(long departmentId, CancellationToken cancellationToken = default)
        {
            return await _context.Positions
                .AnyAsync(x => x.DepartmentId == departmentId && !x.IsDeleted, cancellationToken);
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

            var existingNames = await _context.Departments
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

            var existingIds = await _context.Departments
                .Where(x => idList.Contains(x.PublicId))
                .Select(x => x.PublicId)
                .ToListAsync(cancellationToken);

            return existingIds.ToHashSet();
        }

        public async Task<IEnumerable<Department>> ToListByPublicIdsAsync(
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

            return await _context.Departments
                .Where(x => idList.Contains(x.PublicId))
                .ToListAsync(cancellationToken);
        }
    }
}
