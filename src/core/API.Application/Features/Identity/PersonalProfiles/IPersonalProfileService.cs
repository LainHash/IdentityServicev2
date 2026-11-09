using API.Application.Features.Identity.PersonalProfiles.Commands.CompleteProfile;
using API.Application.Features.Identity.PersonalProfiles.Commands.UpdateByOwner;
using API.Contract.DTOs.Identity.PersonalProfiles;
using API.Domain.Common.Models.Results;

namespace API.Application.Features.Identity.PersonalProfiles
{
    public interface IPersonalProfileService
    {
        Task<Result> CompleteProfileAsync(
            CompleteProfileCommand command,
            CancellationToken cancellationToken = default);

        Task<Result<PersonalProfileResponse>> UpdatePersonalProfileAsync(
            UpdatePersonalProfileCommand command,
            UpdatePersonalProfileSpecification specification,
            CancellationToken cancellationToken = default);
    }
}
