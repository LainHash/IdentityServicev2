using FluentValidation;

namespace API.Application.Features.Personnel.Departments.Commands.Update
{
    public class UpdateDepartmentCommandValidator : AbstractValidator<UpdateDepartmentCommand>
    {
        public UpdateDepartmentCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Department ID is required.");

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

