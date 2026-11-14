namespace API.Contract.DTOs.Identity.PersonalProfiles
{
    public class PersonalProfileResponse : SoftDeletableResponse
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public DateOnly DateOfBirth { get; set; }
        public bool Gender { get; set; }

        public AddressResponse Address { get; set; } = new();

        public string Phone { get; set; } = string.Empty;
        public string CitizenCardId { get; set; } = string.Empty;
    }
}
