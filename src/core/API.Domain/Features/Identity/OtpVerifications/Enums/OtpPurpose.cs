using System.Text.Json.Serialization;

namespace API.Domain.Features.Identity.OtpVerifications.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OtpPurpose
    {
        EmailVerification,
        PasswordReset,
        ChangeEmail,
        Login
    }
}
