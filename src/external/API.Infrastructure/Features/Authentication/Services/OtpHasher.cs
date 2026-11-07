using API.Application.Features.Authentication.Services;
using API.Contract.Settings;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace API.Infrastructure.Features.Authentication.Services
{
    internal class OtpHasher : IOtpHasher
    {
        private readonly string _secret;

        public OtpHasher(IOptions<AppSettings> options)
        {
            _secret = options.Value.OtpSecretKey
                ?? throw new InvalidOperationException(
                    "otp secret key is not configured.");
        }

        public string HashOtp(string otp)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secret));

            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(otp));

            return Convert.ToHexString(hash);
        }

        public bool VerifyOtp(string otp, string storedHash)
        {
            using var hmac = new HMACSHA256(
                Encoding.UTF8.GetBytes(_secret));

            var actualHash = hmac.ComputeHash(
                Encoding.UTF8.GetBytes(otp));

            var expectedHash = Convert.FromHexString(storedHash);

            return CryptographicOperations.FixedTimeEquals(
                actualHash,
                expectedHash);
        }
    }
}
