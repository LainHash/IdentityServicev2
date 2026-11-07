using API.Application.Features.Identity.OtpVerifications.Commands.VerifyOtp;
using API.Domain.Common.Models.Results;
using API.Domain.Features.Identity.Users;

namespace API.Application.Features.Identity.OtpVerifications
{
    public interface IOtpVerificationService
    {
        Task InitializeAsync(
            User user, 
            CancellationToken cancellationToken = default);

        Task<APIResponse> VerifyOtpAsync(
            VerifyOtpCommand command,
            CancellationToken cancellationToken = default);
    }
}
