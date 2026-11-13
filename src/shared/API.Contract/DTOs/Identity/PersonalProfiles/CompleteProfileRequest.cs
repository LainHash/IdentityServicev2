namespace API.Contract.DTOs.Identity.PersonalProfiles
{
    public class CompleteProfileRequest
    {
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public bool Gender { get; set; }
        public AddressRequest Address { get; set; } = new();
        public string Phone { get; set; } = string.Empty;
        public string CitizenCardId { get; set; } = string.Empty;
    }
}
