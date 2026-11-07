using FluentValidation;

namespace API.Application.Features.Authentication.Commands.GoogleLogin
{
    public class GoogleLoginCommandValidator
        : AbstractValidator<GoogleLoginCommand>
    {
        public GoogleLoginCommandValidator()
        {
            RuleFor(x => x.Body.IdToken)
                .NotEmpty()
                .WithMessage("Google IdToken is required.");
        }
    }
}
