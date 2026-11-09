using API.Contract.DTOs.Identity.PersonalProfiles;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Identity.PersonalProfiles.Commands.UpdateByOwner
{
    public record UpdatePersonalProfileCommand(Guid UserId, UpdatePersonalProfileRequest Body)
        : IRequest<Result<PersonalProfileResponse>>
    {
    }
}
