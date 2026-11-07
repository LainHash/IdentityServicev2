using API.Domain.Common.Repositories;

namespace API.Domain.Features.Identity.Users
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> FindByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<User?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<User?> FindByIdWithRoleAsync(Guid id, CancellationToken cancellationToken = default);

        Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken = default);

        Task<User?> FindByIdentifierAsync(string identifier, CancellationToken cancellationToken = default);

        Task<bool> IsExistingEmailAsync(
            Guid? excludePublicId, 
            string email, 
            CancellationToken cancellationToken = default);

        Task<bool> IsExistingUserNameAsync(
            Guid? excludePublicId, 
            string userName, 
            CancellationToken cancellationToken = default);
    }
}
