namespace API.Domain.Features.Identity.PersonalProfiles.ValueObjects
{
    public partial class Address
    {
        public string Street { get; private set; } = string.Empty;
        public string? Ward { get; private set; }
        public string? District { get; private set; }
        public string Province { get; private set; } = string.Empty;
        public string Country { get; private set; } = "Vietnam";
        public string? FullAddress { get; private set; }

        public decimal? Latitude { get; private set; }
        public decimal? Longitude { get; private set; }

        public Guid? AdministrativeDivisionPublicId { get; private set; }
    }

    public partial class Address
    {
        public Address() { }

        public Address(
            string street,
            string? ward,
            string? district,
            string province,
            string country = "Vietnam",
            string? fullAddress = null,
            decimal? latitude = null,
            decimal? longitude = null,
            Guid? administrativeDivisionPublicId = null)
        {
            Street = street;
            Ward = ward;
            District = district;
            Province = province;
            Country = string.IsNullOrWhiteSpace(country) ? "Vietnam" : country;
            FullAddress = string.IsNullOrWhiteSpace(fullAddress)
                ? BuildFullAddress(street, ward, district, province, Country)
                : fullAddress;
            Latitude = latitude;
            Longitude = longitude;
            AdministrativeDivisionPublicId = administrativeDivisionPublicId;
        }

        public Address SetFullAddress(string fullAddress)
        {
            FullAddress = fullAddress;
            return this;
        }

        public Address SetCoordinates(decimal? latitude, decimal? longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
            return this;
        }

        public Address SetAdministrativeDivisionPublicId(Guid? publicId)
        {
            AdministrativeDivisionPublicId = publicId;
            return this;
        }

        public Address Update(
            string street,
            string? ward,
            string? district,
            string province,
            string country = "Vietnam",
            string? fullAddress = null,
            decimal? latitude = null,
            decimal? longitude = null,
            Guid? administrativeDivisionPublicId = null)
        {
            Street = street;
            Ward = ward;
            District = district;
            Province = province;
            Country = string.IsNullOrWhiteSpace(country) ? "Vietnam" : country;
            FullAddress = string.IsNullOrWhiteSpace(fullAddress)
                ? BuildFullAddress(street, ward, district, province, Country)
                : fullAddress;
            Latitude = latitude;
            Longitude = longitude;
            AdministrativeDivisionPublicId = administrativeDivisionPublicId;
            return this;
        }

        public static string BuildFullAddress(string street, string? ward, string? district, string province, string country)
        {
            var parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(street)) parts.Add(street.Trim());
            if (!string.IsNullOrWhiteSpace(ward)) parts.Add(ward.Trim());
            if (!string.IsNullOrWhiteSpace(district)) parts.Add(district.Trim());
            if (!string.IsNullOrWhiteSpace(province)) parts.Add(province.Trim());
            if (!string.IsNullOrWhiteSpace(country)) parts.Add(country.Trim());
            return string.Join(", ", parts);
        }
    }
}

