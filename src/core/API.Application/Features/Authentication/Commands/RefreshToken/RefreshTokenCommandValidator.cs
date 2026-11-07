using FluentValidation;

namespace API.Application.Features.Authentication.Commands.RefreshToken
{
    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(x => x.Body.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required.");
        }
    }
}
