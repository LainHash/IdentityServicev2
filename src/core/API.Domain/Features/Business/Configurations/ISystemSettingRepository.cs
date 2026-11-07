using API.Domain.Common.Repositories;

namespace API.Domain.Features.Business.Configurations
{
    public interface ISystemSettingRepository : IRepository<SystemSetting>
    {
        Task<SystemSetting?> FindByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<SystemSetting?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<SystemSetting?> FindByKeyAsync(string key, CancellationToken cancellationToken = default);
        Task<bool> IsExistingKeyAsync(string key, CancellationToken cancellationToken = default);
        Task<IEnumerable<SystemSetting>> GetByGroupAsync(string group, CancellationToken cancellationToken = default);
        Task<IEnumerable<SystemSetting>> GetPublicSettingsAsync(CancellationToken cancellationToken = default);
    }
}
