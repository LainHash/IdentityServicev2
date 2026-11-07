using API.Application.Features.Authentication.Services;
using API.Application.Features.Business;
using API.Application.Features.Business.Bootstrap.Services;
using API.Contract.Settings;
using API.Domain.Common.Constants;
using API.Domain.Features.Identity.Roles;
using API.Domain.Features.Identity.Users;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace API.Infrastructure.Features.Business.Bootstrap.Seeders
{
    internal class SuperAdminSeeder : IDataSeeder
    {
        public int Order => 2;

        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IOptions<AppSettings> _appSettings;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SuperAdminSeeder> _logger;

        public SuperAdminSeeder(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IPasswordHasher passwordHasher,
            IOptions<AppSettings> appSettings,
            IUnitOfWork unitOfWork,
            ILogger<SuperAdminSeeder> logger)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _appSettings = appSettings;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> ShouldSeedAsync(CancellationToken cancellationToken = default)
        {
            var settings = _appSettings.Value.Bootstrap;

            var existingByUserName = await _userRepository
                .FindByIdentifierAsync(settings.AdminUserName, cancellationToken);

            var existingByEmail = await _userRepository
                .FindByIdentifierAsync(settings.AdminEmail, cancellationToken);

            return existingByUserName is null && existingByEmail is null;
        }

        public async Task<SeederResult> SeedAsync(CancellationToken cancellationToken = default)
        {
            var settings = _appSettings.Value.Bootstrap;

            var existingByUserName = await _userRepository.FindByIdentifierAsync(settings.AdminUserName, cancellationToken);
            var existingByEmail = await _userRepository.FindByIdentifierAsync(settings.AdminEmail, cancellationToken);
            if (existingByUserName is not null || existingByEmail is not null)
            {
                return new SeederResult("SuperAdminSeeder", 0, [settings.AdminUserName]);
            }

            var superAdminRole = await _roleRepository
                                .FindByNameAsync(DefaultRoles.SuperAdmin, cancellationToken)
                              ?? await _roleRepository
                                .FindByNameAsync(DefaultRoles.Admin, cancellationToken)
                              ?? throw new InvalidOperationException("Failed to resolve administrator role.");

            var passwordHash = _passwordHasher.HashPassword(settings.AdminPassword);
            var superAdminUser = new User(
                userName: settings.AdminUserName,
                email: settings.AdminEmail,
                passwordHash: passwordHash,
                roleId: superAdminRole.Id,
                isActive: true
            );

            _userRepository.Add(superAdminUser);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("SuperAdminSeeder: SuperAdmin '{UserName}' created successfully.", settings.AdminUserName);

            return new SeederResult(
                SeederName: "SuperAdminSeeder",
                ItemsCreatedCount: 1,
                ItemsCreated: [settings.AdminUserName]
            );
        }
    }
}
