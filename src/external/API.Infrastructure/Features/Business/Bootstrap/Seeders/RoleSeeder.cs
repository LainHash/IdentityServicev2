using API.Application.Features.Business;
using API.Application.Features.Business.Bootstrap.Services;
using API.Domain.Common.Constants;
using API.Domain.Features.Identity.Roles;
using Microsoft.Extensions.Logging;

namespace API.Infrastructure.Features.Business.Bootstrap.Seeders
{
    internal class RoleSeeder : IDataSeeder
    {
        public int Order => 1;

        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RoleSeeder> _logger;

        public RoleSeeder(
            IRoleRepository roleRepository,
            IUnitOfWork unitOfWork,
            ILogger<RoleSeeder> logger)
        {
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> ShouldSeedAsync(CancellationToken cancellationToken = default)
        {
            foreach (var (name, _) in DefaultRoles.All)
            {
                if (!await _roleRepository.IsExistingNameAsync(name, cancellationToken))
                    return true;
            }
            return false;
        }

        public async Task<SeederResult> SeedAsync(CancellationToken cancellationToken = default)
        {
            var rolesCreated = new List<string>();

            foreach (var (name, description) in DefaultRoles.All)
            {
                var exists = await _roleRepository.IsExistingNameAsync(name, cancellationToken);
                if (!exists)
                {
                    var role = new Role(name, description);
                    _roleRepository.Add(role);
                    rolesCreated.Add(name);
                }
            }

            if (rolesCreated.Count > 0)
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("RoleSeeder: {Count} roles created: {Roles}", rolesCreated.Count, string.Join(", ", rolesCreated));
            }

            return new SeederResult(
                SeederName: "RoleSeeder",
                ItemsCreatedCount: rolesCreated.Count,
                ItemsCreated: DefaultRoles.All.Select(r => r.Name).ToList()
            );
        }
    }
}
