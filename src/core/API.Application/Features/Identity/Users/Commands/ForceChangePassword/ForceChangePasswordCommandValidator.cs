using FluentValidation;

namespace API.Application.Features.Identity.Users.Commands.ForceChangePassword
{
    public class ForceChangePasswordCommandValidator
        : AbstractValidator<ForceChangePasswordCommand>
    {
        public ForceChangePasswordCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("User ID is required.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MinimumLength(6).WithMessage("New password must be at least 6 characters.");
        }
    }
}
