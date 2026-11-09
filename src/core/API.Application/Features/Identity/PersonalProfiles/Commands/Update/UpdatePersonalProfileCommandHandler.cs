using API.Contract.DTOs.Identity.PersonalProfiles;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Identity.PersonalProfiles.Commands.UpdateByOwner
{
    internal class UpdatePersonalProfileCommandHandler(IPersonalProfileService personalProfileService)
                : IRequestHandler<UpdatePersonalProfileCommand, Result<PersonalProfileResponse>>
    {
        private readonly IPersonalProfileService _personalProfileService = personalProfileService;

        public async Task<Result<PersonalProfileResponse>> Handle(UpdatePersonalProfileCommand request, CancellationToken cancellationToken)
        {
            var specification = new UpdatePersonalProfileSpecification(request);
            var response = await _personalProfileService.UpdatePersonalProfileAsync(request, specification, cancellationToken);
            return response;
        }
    }
}
