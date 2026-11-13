using API.Domain.Common.Repositories;

namespace API.Domain.Features.Personnel.Employees
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<Employee?> FindByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<Employee?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Employee?> FindByUserIdAsync(long userId, CancellationToken cancellationToken = default);
        Task<Employee?> FindByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<Employee?> FindWithDetailsByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
