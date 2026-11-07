using API.Contract.DTOs.Identity.OtpVerifications;
using API.Domain.Common.Models.Results;
using API.Domain.Enums;
using MediatR;

namespace API.Application.Features.Identity.OtpVerifications.Commands.VerifyOtp
{
    public record VerifyOtpCommand(VerifyOtpRequest Body)
        : IRequest<APIResponse>
    {
    }
}
