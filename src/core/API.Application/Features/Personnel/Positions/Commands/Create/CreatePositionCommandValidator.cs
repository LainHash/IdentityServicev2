using FluentValidation;

namespace API.Application.Features.Personnel.Positions.Commands.Create
{
    public class CreatePositionCommandValidator : AbstractValidator<CreatePositionCommand>
    {
        public CreatePositionCommandValidator()
        {
            RuleFor(x => x.Body.DepartmentId)
                .NotEmpty().WithMessage("Department ID is required.");

            RuleFor(x => x.Body.Name)
                .NotEmpty().WithMessage("Position name is required.")
                .MaximumLength(50).WithMessage("Position name must not exceed 50 characters.");

            RuleFor(x => x.Body.Code)
                .MaximumLength(50).WithMessage("Position code must not exceed 50 characters.");

            RuleFor(x => x.Body.Description)
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");
        }
    }
}

