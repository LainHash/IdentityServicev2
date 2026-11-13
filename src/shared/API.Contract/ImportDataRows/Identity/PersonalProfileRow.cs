namespace API.Contract.ImportDataRows.Identity
{
    public class PersonalProfileRow
    {
        public Guid PublicId { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string DateOfBirth { get; set; } = string.Empty;
        public string Gender { get; set; } = "Male";
        public string Phone { get; set; } = string.Empty;
        public string CitizenCardId { get; set; } = string.Empty;

        // Address fields
        public string Street { get; set; } = string.Empty;
        public string? Ward { get; set; }
        public string? District { get; set; }
        public string Province { get; set; } = string.Empty;
        public string Country { get; set; } = "Vietnam";
        public string? FullAddress { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public Guid? AdministrativeDivisionPublicId { get; set; }
    }
}

