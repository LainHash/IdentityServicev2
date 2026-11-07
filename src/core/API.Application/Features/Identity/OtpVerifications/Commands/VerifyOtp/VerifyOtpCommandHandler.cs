using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Identity.OtpVerifications.Commands.VerifyOtp
{
    internal class VerifyOtpCommandHandler(IOtpVerificationService otpVerificationService)
                : IRequestHandler<VerifyOtpCommand, APIResponse>
    {
        private readonly IOtpVerificationService _otpVerificationService = otpVerificationService;

        public async Task<APIResponse> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
        {
            return await _otpVerificationService.VerifyOtpAsync(request, cancellationToken);
        }
    }
}
