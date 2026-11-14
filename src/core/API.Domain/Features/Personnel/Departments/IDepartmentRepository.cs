using API.Domain.Common.Repositories;

namespace API.Domain.Features.Personnel.Departments
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        Task<Department?> FindByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<Department?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<bool> IsExistingNameAsync(
            Guid? excludePublicId, 
            string name, 
            CancellationToken cancellationToken = default);
        Task<bool> IsExistingCodeAsync(
            Guid? excludePublicId, 
            string code, 
            CancellationToken cancellationToken = default);
        Task<bool> HasActivePositionsAsync(
            long departmentId, 
            CancellationToken cancellationToken = default);
        Task<HashSet<string>> GetExistingNamesAsync(
            IEnumerable<string> names, 
            CancellationToken cancellationToken = default);
        Task<HashSet<Guid>> GetExistingPublicIdsAsync(
            IEnumerable<Guid> publicIds, 
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Department>> ToListByPublicIdsAsync(
            IEnumerable<Guid> publicIds, 
            CancellationToken cancellationToken = default);
    }
}
