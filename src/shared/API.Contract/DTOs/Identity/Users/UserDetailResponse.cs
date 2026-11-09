using API.Contract.DTOs.Identity.PersonalProfiles;

namespace API.Contract.DTOs.Identity.Users
{
    public class UserDetailResponse : UserResponse
    {
        public PersonalProfileResponse? PersonalProfile { get; set; }
    }
}
