using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Identity.OtpVerifications.Commands.VerifyOtp
{
    internal class VerifyOtpCommandHandler(IOtpVerificationService otpVerificationService)
                : IRequestHandler<VerifyOtpCommand, Result>
    {
        private readonly IOtpVerificationService _otpVerificationService = otpVerificationService;

        public async Task<Result> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
        {
            return await _otpVerificationService.VerifyOtpAsync(request, cancellationToken);
        }
    }
}
