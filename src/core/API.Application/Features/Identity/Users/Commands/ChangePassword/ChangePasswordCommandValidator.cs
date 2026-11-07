using FluentValidation;

namespace API.Application.Features.Identity.Users.Commands.ChangePassword
{
    public class ChangePasswordCommandValidator
        : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.");

            RuleFor(x => x.Body.OldPassword)
                .NotEmpty().WithMessage("Current password is required.");

            RuleFor(x => x.Body.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MinimumLength(6).WithMessage("New password must be at least 6 characters.")
                .NotEqual(x => x.Body.OldPassword).WithMessage("New password cannot be the same as current password.");

            RuleFor(x => x.Body.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm password is required.")
                .Equal(x => x.Body.NewPassword).WithMessage("Confirm password does not match new password.");
        }
    }
}
