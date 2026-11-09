using Microsoft.EntityFrameworkCore;
using API.Infrastructure.Context;
using API.Domain.Features.Identity.PersonalProfiles;
using API.Infrastructure.Common.Repositories;

namespace API.Infrastructure.Features.Identity.PersonalProfiles
{
    internal class PersonalProfileRepository(AppDbContext context)
        : Repository<PersonalProfile>(context), IPersonalProfileRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<PersonalProfile?> FindByUserIdAsync(
            long userId,
            CancellationToken cancellationToken = default)
        {
            return await _context.PersonalProfiles
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        }
    }
}
