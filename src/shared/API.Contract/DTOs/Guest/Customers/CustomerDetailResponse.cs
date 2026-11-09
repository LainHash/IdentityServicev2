using API.Contract.DTOs.Identity.Users;

namespace API.Contract.DTOs.Guest.Customers
{
    public class CustomerDetailResponse : CustomerResponse
    {
        public UserDetailResponse User { get; set; } = null!;
    }
}
