using FluentValidation;

namespace API.Application.Features.Personnel.Departments.Commands.Create
{
    public class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
    {
        public CreateDepartmentCommandValidator()
        {
            RuleFor(x => x.Body.Name)
                .NotEmpty().WithMessage("Department name is required.")
                .MaximumLength(50).WithMessage("Department name must not exceed 50 characters.");

            RuleFor(x => x.Body.Code)
                .MaximumLength(50).WithMessage("Department code must not exceed 50 characters.");

            RuleFor(x => x.Body.Description)
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");
        }
    }
}

