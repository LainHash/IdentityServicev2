namespace API.Application.Features.Authentication.Services
{
    public interface IOtpHasher
    {
        string HashOtp(string otp);
        bool VerifyOtp(string inputOtp, string storedOtp);
    }
}
