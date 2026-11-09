using API.Contract.DTOs.Identity.PersonalProfiles;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Identity.PersonalProfiles.Commands.CompleteProfile
{
    public record CompleteProfileCommand(CompleteProfileRequest Body)
        : IRequest<Result>
    {
    }
}
