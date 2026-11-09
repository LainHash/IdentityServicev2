using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Identity.PersonalProfiles.Commands.CompleteProfile
{
    internal class CompleteProfileCommandHandler(IPersonalProfileService personalProfileService)
                : IRequestHandler<CompleteProfileCommand, Result>
    {
        private readonly IPersonalProfileService _personalProfileService = personalProfileService;

        public async Task<Result> Handle(CompleteProfileCommand request, CancellationToken cancellationToken)
        {
            var response = await _personalProfileService.CompleteProfileAsync(request, cancellationToken);
            return response;
        }
    }
}
