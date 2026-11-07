using API.Domain.Common.Repositories;
using API.Domain.Entities.Identity.OtpVerifications;
using API.Domain.Features.Identity.OtpVerifications.Enums;

namespace API.Domain.Features.Identity.OtpVerifications
{
    public interface IOtpVerificationRepository : IRepository<OtpVerification>
    {
        Task<OtpVerification?> FindActiveAsync(long userId, OtpPurpose purpose, CancellationToken cancellationToken = default);
    }
}
