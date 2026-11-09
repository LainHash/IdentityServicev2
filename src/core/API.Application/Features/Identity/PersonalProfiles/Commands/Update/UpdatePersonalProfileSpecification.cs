using API.Domain.Common.Specifications;
using API.Domain.Features.Identity.PersonalProfiles;

namespace API.Application.Features.Identity.PersonalProfiles.Commands.UpdateByOwner
{
    public class UpdatePersonalProfileSpecification
        : BaseSpecification<PersonalProfile>
    {
        public UpdatePersonalProfileSpecification(UpdatePersonalProfileCommand command)
        {
            AddInclude(x => x.User);

            Criteria = pp => pp.User.PublicId == command.UserId;
        }
    }
}
