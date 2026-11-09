using API.Domain.Common.Repositories;

namespace API.Domain.Features.Identity.PersonalProfiles
{
    public interface IPersonalProfileRepository : IRepository<PersonalProfile>
    {
        Task<PersonalProfile?> FindByUserIdAsync(
            long userId, 
            CancellationToken cancellationToken = default);
    }
}
