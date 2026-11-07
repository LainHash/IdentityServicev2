using Microsoft.EntityFrameworkCore;
using API.Infrastructure.Contexts.Data;
using API.Domain.Features.Identity.OtpVerifications;
using API.Infrastructure.Common.Repositories;
using API.Domain.Entities.Identity.OtpVerifications;
using API.Domain.Features.Identity.OtpVerifications.Enums;

namespace API.Infrastructure.Features.Identity.OtpVerifications
{
    internal class OtpVerificationRepository(AppDbContext context)
        : Repository<OtpVerification>(context), IOtpVerificationRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<OtpVerification?> FindActiveAsync(long userId, OtpPurpose purpose, CancellationToken cancellationToken = default)
        {
            return await _context.OtpVerifications
                .Where(x => x.UserId == userId &&
                            x.Purpose == purpose &&
                            x.IsAvailable)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
