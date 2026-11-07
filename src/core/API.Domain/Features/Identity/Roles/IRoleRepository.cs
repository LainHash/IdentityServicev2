using API.Domain.Common.Repositories;

namespace API.Domain.Features.Identity.Roles
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role?> FindByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<Role?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<Role?> FindByNameAsync(string name, CancellationToken cancellationToken = default);

        Task<bool> IsExistingNameAsync(string name, CancellationToken cancellationToken = default);

        Task<bool> IsExistingNameAsync(
            Guid? excludePublicId,
            string name,
            CancellationToken cancellationToken = default);
    }
}
