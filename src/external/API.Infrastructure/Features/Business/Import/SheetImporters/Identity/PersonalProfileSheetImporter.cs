using API.Application.Features.Business;
using API.Contract.DTOs.Business.Import;
using API.Contract.ImportDataRows.Identity;
using API.Domain.Features.Identity.PersonalProfiles;
using API.Domain.Features.Identity.PersonalProfiles.ValueObjects;
using API.Domain.Features.Identity.Users;
using System.Globalization;

namespace API.Infrastructure.Features.Business.Import.SheetImporters.Identity
{
    internal class PersonalProfileSheetImporter : BaseSheetImporter<PersonalProfileRow>
    {
        private readonly IPersonalProfileRepository _personalProfileRepository;
        private readonly IUserRepository _userRepository;

        private readonly Dictionary<string, User> _userByEmail = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<Guid, User> _userByPublicId = [];
        private readonly HashSet<long> _existingUserIdsWithProfile = [];
        private readonly HashSet<Guid> _existingPublicIds = [];

        private readonly HashSet<string> _seenUserEmails = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<Guid> _seenPublicIds = [];

        public PersonalProfileSheetImporter(
            IPersonalProfileRepository personalProfileRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _personalProfileRepository = personalProfileRepository;
            _userRepository = userRepository;
        }

        public override string SheetName => "PersonalProfiles";

        protected override async Task BeforeProcessRowsAsync(
            IReadOnlyList<PersonalProfileRow> rows,
            CancellationToken cancellationToken)
        {
            _userByEmail.Clear();
            _userByPublicId.Clear();
            _existingUserIdsWithProfile.Clear();
            _existingPublicIds.Clear();

            _seenUserEmails.Clear();
            _seenPublicIds.Clear();

            var users = await _userRepository.ToListAsync(cancellationToken);
            foreach (var user in users)
            {
                if (!string.IsNullOrWhiteSpace(user.Email))
                    _userByEmail[user.Email] = user;
                if (user.PublicId != Guid.Empty)
                    _userByPublicId[user.PublicId] = user;
            }

            var profiles = await _personalProfileRepository.ToListAsync(cancellationToken);
            foreach (var profile in profiles)
            {
                _existingUserIdsWithProfile.Add(profile.UserId);
                if (profile.PublicId != Guid.Empty)
                    _existingPublicIds.Add(profile.PublicId);
            }
        }

        protected override Task ProcessRowAsync(
            PersonalProfileRow row,
            int rowIndex,
            ImportResultResponse result,
            CancellationToken cancellationToken)
        {
            var userEmail = row.UserEmail?.Trim();
            if (string.IsNullOrWhiteSpace(userEmail))
            {
                result.AddError(rowIndex, "UserEmail is required.");
                return Task.CompletedTask;
            }

            if (!_userByEmail.TryGetValue(userEmail, out var user))
            {
                result.AddError(rowIndex, $"User with email '{userEmail}' does not exist.");
                return Task.CompletedTask;
            }

            if (_existingUserIdsWithProfile.Contains(user.Id))
            {
                result.AddError(rowIndex, $"User with email '{userEmail}' already has a PersonalProfile.");
                return Task.CompletedTask;
            }

            if (!_seenUserEmails.Add(userEmail))
            {
                result.AddError(rowIndex, $"User with email '{userEmail}' is duplicated in the file.");
                return Task.CompletedTask;
            }

            var firstName = row.FirstName?.Trim();
            if (string.IsNullOrWhiteSpace(firstName))
            {
                result.AddError(rowIndex, "FirstName is required.");
                return Task.CompletedTask;
            }

            var lastName = row.LastName?.Trim();
            if (string.IsNullOrWhiteSpace(lastName))
            {
                result.AddError(rowIndex, "LastName is required.");
                return Task.CompletedTask;
            }

            if (!TryParseDateOfBirth(row.DateOfBirth, out var dateOfBirth))
            {
                result.AddError(rowIndex, $"DateOfBirth '{row.DateOfBirth}' is invalid. Expected formats: yyyy-MM-dd, dd/MM/yyyy.");
                return Task.CompletedTask;
            }

            var gender = ParseGender(row.Gender);

            var phone = row.Phone?.Trim();
            if (string.IsNullOrWhiteSpace(phone))
            {
                result.AddError(rowIndex, "Phone is required.");
                return Task.CompletedTask;
            }

            var citizenCardId = row.CitizenCardId?.Trim();
            if (string.IsNullOrWhiteSpace(citizenCardId))
            {
                result.AddError(rowIndex, "CitizenCardId is required.");
                return Task.CompletedTask;
            }

            var street = row.Street?.Trim();
            if (string.IsNullOrWhiteSpace(street))
            {
                result.AddError(rowIndex, "Street address is required.");
                return Task.CompletedTask;
            }

            var province = row.Province?.Trim();
            if (string.IsNullOrWhiteSpace(province))
            {
                result.AddError(rowIndex, "Province is required.");
                return Task.CompletedTask;
            }

            var country = string.IsNullOrWhiteSpace(row.Country) ? "Vietnam" : row.Country.Trim();
            var fullAddress = string.IsNullOrWhiteSpace(row.FullAddress)
                ? Address.BuildFullAddress(street, row.Ward, row.District, province, country)
                : row.FullAddress.Trim();

            if (row.PublicId != Guid.Empty)
            {
                if (_existingPublicIds.Contains(row.PublicId))
                {
                    result.AddError(rowIndex, $"PersonalProfile with public id '{row.PublicId}' already exists.");
                    return Task.CompletedTask;
                }

                if (!_seenPublicIds.Add(row.PublicId))
                {
                    result.AddError(rowIndex, $"PersonalProfile with public id '{row.PublicId}' is duplicated in the file.");
                    return Task.CompletedTask;
                }
            }

            var address = new Address(
                street,
                row.Ward?.Trim(),
                row.District?.Trim(),
                province,
                country,
                fullAddress,
                row.Latitude,
                row.Longitude,
                row.AdministrativeDivisionPublicId != Guid.Empty ? row.AdministrativeDivisionPublicId : null);

            var profile = new PersonalProfile(
                firstName,
                lastName,
                dateOfBirth,
                gender,
                address,
                phone,
                citizenCardId,
                user.Id);

            if (row.PublicId != Guid.Empty)
            {
                typeof(API.Domain.Common.Entities.Entity)
                    .GetProperty(nameof(profile.PublicId))?
                    .SetValue(profile, row.PublicId);
            }

            _personalProfileRepository.Add(profile);
            _existingUserIdsWithProfile.Add(user.Id);

            result.SuccessCount++;
            return Task.CompletedTask;
        }

        private static bool TryParseDateOfBirth(string? input, out DateOnly date)
        {
            date = default;
            if (string.IsNullOrWhiteSpace(input)) return false;

            var formats = new[] { "yyyy-MM-dd", "dd/MM/yyyy", "d/M/yyyy", "MM/dd/yyyy", "yyyy/MM/dd" };
            if (DateOnly.TryParseExact(input.Trim(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                return true;
            }

            if (DateTime.TryParse(input.Trim(), CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
            {
                date = DateOnly.FromDateTime(dt);
                return true;
            }

            return false;
        }

        private static bool ParseGender(string? input)
        {
            if (string.IsNullOrWhiteSpace(input)) return true; // Default Male

            var clean = input.Trim().ToLowerInvariant();
            if (clean is "female" or "nữ" or "nu" or "false" or "0")
            {
                return false;
            }

            return true;
        }
    }
}

