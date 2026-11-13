using API.Domain.Common.Repositories;

namespace API.Domain.Features.Personnel.Positions
{
    public interface IPositionRepository : IRepository<Position>
    {
        Task<Position?> FindByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<Position?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<bool> IsExistingNameAsync(Guid id, string name, CancellationToken cancellationToken = default);
        Task<HashSet<string>> GetExistingNamesAsync(IEnumerable<string> names, CancellationToken cancellationToken = default);
        Task<HashSet<Guid>> GetExistingPublicIdsAsync(IEnumerable<Guid> publicIds, CancellationToken cancellationToken = default);
    }
}
