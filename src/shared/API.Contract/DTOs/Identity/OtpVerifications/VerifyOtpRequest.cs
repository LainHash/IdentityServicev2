using API.Domain.Features.Identity.OtpVerifications.Enums;

namespace API.Contract.DTOs.Identity.OtpVerifications
{
    public class VerifyOtpRequest
    {
        public string Email { get; set; } = null!;
        public string Otp { get; set; } = null!;
        public OtpPurpose Purpose { get; set; }
    }
}
