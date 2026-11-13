namespace API.Contract.DTOs.Identity.PersonalProfiles
{
    public class AddressResponse
    {
        public string Street { get; set; } = string.Empty;
        public string? Ward { get; set; }
        public string? District { get; set; }
        public string Province { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string? FullAddress { get; set; }

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        public Guid? AdministrativeDivisionPublicId { get; set; }
    }
}

