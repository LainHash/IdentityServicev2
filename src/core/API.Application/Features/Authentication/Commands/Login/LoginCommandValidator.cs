using FluentValidation;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace API.Application.Features.Authentication.Commands.Login
{
    public class LoginCommandValidator
        : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Body.Identifier)
                .NotEmpty()
                .WithMessage("Email or Username is required.")
                .Must(identifier =>
                {
                    if (string.IsNullOrWhiteSpace(identifier))
                        return false;

                    identifier = identifier.Trim();

                    if (MailAddress.TryCreate(identifier, out _))
                        return true;

                    return Regex.IsMatch(identifier, @"^[a-zA-Z0-9_-]+$");
                })
                .WithMessage("Identifier must be a valid email or username without special characters.");

            RuleFor(x => x.Body.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}
