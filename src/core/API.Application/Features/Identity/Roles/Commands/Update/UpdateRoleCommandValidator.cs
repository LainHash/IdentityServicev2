using FluentValidation;

namespace API.Application.Features.Identity.Roles.Commands.Update
{
    public class UpdateRoleCommandValidator
        : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Role id is required.");

            RuleFor(x => x.Body.Name)
                .NotEmpty().WithMessage("Role name is required.")
                .MaximumLength(50).WithMessage("Role name must not exceed 50 characters.");

            RuleFor(x => x.Body.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
        }
    }
}

