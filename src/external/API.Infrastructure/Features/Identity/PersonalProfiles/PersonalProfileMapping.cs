using API.Contract.DTOs.Identity.PersonalProfiles;
using API.Domain.Features.Identity.PersonalProfiles;
using API.Domain.Features.Identity.PersonalProfiles.ValueObjects;
using AutoMapper;

namespace API.Infrastructure.Features.Identity.PersonalProfiles
{
    internal class PersonalProfileMapping : Profile
    {
        public PersonalProfileMapping()
        {
            CreateMap<AddressRequest, Address>()
                .ConstructUsing(src => new Address(
                    src.Street,
                    src.Ward,
                    src.District,
                    src.Province,
                    src.Country,
                    src.FullAddress,
                    src.Latitude,
                    src.Longitude,
                    src.AdministrativeDivisionPublicId))
                .AfterMap((src, dest) =>
                {
                    if (string.IsNullOrWhiteSpace(dest.FullAddress))
                    {
                        dest.SetFullAddress(Address.BuildFullAddress(dest.Street, dest.Ward, dest.District, dest.Province, dest.Country));
                    }
                });

            CreateMap<Address, AddressResponse>();

            CreateMap<CompleteProfileRequest, PersonalProfile>();

            CreateMap<PersonalProfile, PersonalProfileResponse>();

            CreateMap<CreatePersonalProfileRequest, PersonalProfile>();

            CreateMap<UpdatePersonalProfileRequest, PersonalProfile>();
        }
    }
}
