using API.Domain.Common.Entities;
using API.Domain.Features.Identity.PersonalProfiles.ValueObjects;
using API.Domain.Features.Identity.Users;

namespace API.Domain.Features.Identity.PersonalProfiles
{
    public partial class PersonalProfile : SoftDeletableEntity
    {
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;

        public DateOnly DateOfBirth { get; private set; }
        public bool Gender { get; private set; }

        public Address Address { get; private set; } = new();

        public string Phone { get; private set; } = string.Empty;
        public string CitizenCardId { get; private set; } = string.Empty;

        public long UserId { get; private set; }

        public User User { get; private set; } = null!;
    }

    public partial class PersonalProfile
    {
        public PersonalProfile()
        {

        }

        public PersonalProfile(
            string firstName,
            string lastName,
            DateOnly dateOfBirth,
            bool gender,
            Address address,
            string phone,
            string citizenCardId,
            long userId)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Address = address ?? new Address();
            Phone = phone;
            CitizenCardId = citizenCardId;
            UserId = userId;
        }

        public PersonalProfile Update(
            string firstName,
            string lastName,
            DateOnly dateOfBirth,
            bool gender,
            Address address,
            string phone,
            string citizenCardId)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Address = address ?? new Address();
            Phone = phone;
            CitizenCardId = citizenCardId;
            return this;
        }

        public PersonalProfile SetAddress(Address address)
        {
            Address = address ?? new Address();
            return this;
        }

        public PersonalProfile SetUser(long userId)
        {
            UserId = userId;
            return this;
        }
    }
}
