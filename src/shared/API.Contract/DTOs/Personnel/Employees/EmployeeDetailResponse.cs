using API.Contract.DTOs.Identity.Users;

namespace API.Contract.DTOs.Personnel.Employees
{
    public class EmployeeDetailResponse : EmployeeResponse
    {
        public UserDetailResponse User { get; set; } = null!;
    }
}
