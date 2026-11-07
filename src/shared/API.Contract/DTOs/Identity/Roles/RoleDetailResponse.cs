using API.Contract.DTOs.Identity.Users;

namespace API.Contract.DTOs.Identity.Roles
{
    public class RoleDetailResponse : RoleResponse
    {
        public IEnumerable<UserResponse> Users { get; set; } = [];
    }
}

