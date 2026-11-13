using API.Contract.DTOs.Identity.PersonalProfiles;
using FluentValidation;

namespace API.Application.Features.Identity.PersonalProfiles.Validators
{
    public class AddressValidator : AbstractValidator<AddressRequest>
    {
        public AddressValidator()
        {
            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Street address is required.")
                .MaximumLength(200).WithMessage("Street address must not exceed 200 characters.");

            RuleFor(x => x.Province)
                .NotEmpty().WithMessage("Province / City is required.")
                .MaximumLength(100).WithMessage("Province / City must not exceed 100 characters.");

            RuleFor(x => x.Ward)
                .MaximumLength(100).WithMessage("Ward must not exceed 100 characters.")
                .When(x => !string.IsNullOrEmpty(x.Ward));

            RuleFor(x => x.District)
                .MaximumLength(100).WithMessage("District must not exceed 100 characters.")
                .When(x => !string.IsNullOrEmpty(x.District));

            RuleFor(x => x.Country)
                .MaximumLength(50).WithMessage("Country must not exceed 50 characters.")
                .When(x => !string.IsNullOrEmpty(x.Country));

            RuleFor(x => x.FullAddress)
                .MaximumLength(500).WithMessage("Full address must not exceed 500 characters.")
                .When(x => !string.IsNullOrEmpty(x.FullAddress));

            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90.")
                .When(x => x.Latitude.HasValue);

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180.")
                .When(x => x.Longitude.HasValue);
        }
    }
}

