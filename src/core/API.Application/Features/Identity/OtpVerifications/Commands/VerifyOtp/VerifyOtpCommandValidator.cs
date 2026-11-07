using FluentValidation;

namespace API.Application.Features.Identity.OtpVerifications.Commands.VerifyOtp
{
    public class VerifyOtpCommandValidator
        : AbstractValidator<VerifyOtpCommand>
    {
        public VerifyOtpCommandValidator()
        {
            RuleFor(x => x.Body.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email is required.");

            RuleFor(x => x.Body.Otp)
                .NotEmpty().WithMessage("OTP code is required.")
                .Length(6).WithMessage("OTP code must be 6 characters.")
                .Matches(@"^\d{6}$").WithMessage("OTP code must consist of 6 digits.");

            RuleFor(x => x.Body.Purpose)
                .IsInEnum().WithMessage("Invalid OTP purpose.");
        }
    }
}

