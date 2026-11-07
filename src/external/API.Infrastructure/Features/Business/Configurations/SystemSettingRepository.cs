using API.Domain.Features.Business.Configurations;
using API.Infrastructure.Common.Repositories;
using API.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure.Features.Business.Configurations
{
    internal class SystemSettingRepository(AppDbContext context)
        : Repository<SystemSetting>(context), ISystemSettingRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<SystemSetting?> FindByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _context.SystemSettings
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<SystemSetting?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.SystemSettings
                .FirstOrDefaultAsync(x => x.PublicId == id, cancellationToken);
        }

        public async Task<SystemSetting?> FindByKeyAsync(string key, CancellationToken cancellationToken = default)
        {
            return await _context.SystemSettings
                .FirstOrDefaultAsync(x => x.Key == key, cancellationToken);
        }

        public async Task<bool> IsExistingKeyAsync(string key, CancellationToken cancellationToken = default)
        {
            return await _context.SystemSettings
                .AnyAsync(x => x.Key == key, cancellationToken);
        }

        public async Task<IEnumerable<SystemSetting>> GetByGroupAsync(string group, CancellationToken cancellationToken = default)
        {
            return await _context.SystemSettings
                .Where(x => x.Group == group)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<SystemSetting>> GetPublicSettingsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SystemSettings
                .Where(x => x.IsPublic)
                .ToListAsync(cancellationToken);
        }
    }
}
