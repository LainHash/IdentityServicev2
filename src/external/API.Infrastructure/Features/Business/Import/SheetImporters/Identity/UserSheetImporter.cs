using API.Application.Features.Authentication.Services;
using API.Application.Features.Business;
using API.Contract.DTOs.Business.Import;
using API.Contract.ImportDataRows.Identity;
using API.Domain.Features.Identity.Roles;
using API.Domain.Features.Identity.Users;
using AutoMapper;

namespace API.Infrastructure.Features.Business.Import.SheetImporters.Identity
{
    internal class UserSheetImporter : BaseSheetImporter<UserRow>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;

        private readonly Dictionary<string, Role> _roleByName = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _existingEmails = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _existingUserNames = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<Guid> _existingPublicIds = [];

        private readonly HashSet<string> _seenEmails = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _seenUserNames = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<Guid> _seenPublicIds = [];

        public UserSheetImporter(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IPasswordHasher passwordHasher,
            IUnitOfWork unitOfWork,
            IMapper mapper) : base(unitOfWork)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public override string SheetName => "Users";

        protected override async Task BeforeProcessRowsAsync(
            IReadOnlyList<UserRow> rows,
            CancellationToken cancellationToken)
        {
            _roleByName.Clear();
            _existingEmails.Clear();
            _existingUserNames.Clear();
            _existingPublicIds.Clear();

            _seenEmails.Clear();
            _seenUserNames.Clear();
            _seenPublicIds.Clear();

            var roles = await _roleRepository.ToListAsync(cancellationToken);
            foreach (var role in roles)
            {
                _roleByName[role.Name] = role;
            }

            var users = await _userRepository.ToListAsync(cancellationToken);
            foreach (var user in users)
            {
                if (!string.IsNullOrWhiteSpace(user.Email))
                    _existingEmails.Add(user.Email);
                if (!string.IsNullOrWhiteSpace(user.UserName))
                    _existingUserNames.Add(user.UserName);
                if (user.PublicId != Guid.Empty)
                    _existingPublicIds.Add(user.PublicId);
            }
        }

        protected override Task ProcessRowAsync(
            UserRow row,
            int rowIndex,
            ImportResultResponse result,
            CancellationToken cancellationToken)
        {
            var userName = row.UserName?.Trim();
            if (string.IsNullOrWhiteSpace(userName))
            {
                result.AddError(rowIndex, "UserName is required and must not be empty.");
                return Task.CompletedTask;
            }

            if (userName.Length > 50)
            {
                result.AddError(rowIndex, "UserName must not exceed 50 characters.");
                return Task.CompletedTask;
            }

            var email = row.Email?.Trim();
            if (string.IsNullOrWhiteSpace(email))
            {
                result.AddError(rowIndex, "Email is required and must not be empty.");
                return Task.CompletedTask;
            }

            if (email.Length > 100 || !email.Contains('@'))
            {
                result.AddError(rowIndex, "Email is invalid or exceeds 100 characters.");
                return Task.CompletedTask;
            }

            var roleName = row.RoleName?.Trim();
            if (string.IsNullOrWhiteSpace(roleName))
            {
                result.AddError(rowIndex, "RoleName is required.");
                return Task.CompletedTask;
            }

            if (!_roleByName.TryGetValue(roleName, out var role))
            {
                result.AddError(rowIndex, $"Role '{roleName}' does not exist.");
                return Task.CompletedTask;
            }

            if (_existingEmails.Contains(email))
            {
                result.AddError(rowIndex, $"User with email '{email}' already exists.");
                return Task.CompletedTask;
            }

            if (!_seenEmails.Add(email))
            {
                result.AddError(rowIndex, $"User with email '{email}' is duplicated in the file.");
                return Task.CompletedTask;
            }

            if (_existingUserNames.Contains(userName))
            {
                result.AddError(rowIndex, $"User with username '{userName}' already exists.");
                return Task.CompletedTask;
            }

            if (!_seenUserNames.Add(userName))
            {
                result.AddError(rowIndex, $"User with username '{userName}' is duplicated in the file.");
                return Task.CompletedTask;
            }

            if (row.PublicId != Guid.Empty)
            {
                if (_existingPublicIds.Contains(row.PublicId))
                {
                    result.AddError(rowIndex, $"User with public id '{row.PublicId}' already exists.");
                    return Task.CompletedTask;
                }

                if (!_seenPublicIds.Add(row.PublicId))
                {
                    result.AddError(rowIndex, $"User with public id '{row.PublicId}' is duplicated in the file.");
                    return Task.CompletedTask;
                }
            }

            var rawPassword = string.IsNullOrWhiteSpace(row.Password) ? "Password123@" : row.Password.Trim();
            var passwordHash = _passwordHasher.HashPassword(rawPassword);

            var user = User.Create(
                userName,
                email,
                passwordHash,
                role.Id,
                isActive: row.IsActive,
                isConfirmed: row.IsConfirmed);

            if (row.IsLocked)
            {
                user.SetLocked(true);
            }

            // Set PublicId if specified in file
            if (row.PublicId != Guid.Empty)
            {
                typeof(API.Domain.Common.Entities.Entity)
                    .GetProperty(nameof(user.PublicId))?
                    .SetValue(user, row.PublicId);
            }

            _userRepository.Add(user);
            _existingEmails.Add(email);
            _existingUserNames.Add(userName);

            result.SuccessCount++;
            return Task.CompletedTask;
        }
    }
}

