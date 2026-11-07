using FluentValidation;

namespace API.Application.Features.Business.Import.Commands.ImportExcel
{
    public class ImportExcelCommandValidator
        : AbstractValidator<ImportExcelCommand>
    {
        public ImportExcelCommandValidator()
        {
            RuleFor(x => x.FileStream)
                .NotNull().WithMessage("File is required.")
                .Must(s => s != null && s.Length > 0)
                    .WithMessage("File must not be empty.");
        }
    }
}
