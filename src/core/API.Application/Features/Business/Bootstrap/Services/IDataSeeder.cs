namespace API.Application.Features.Business.Bootstrap.Services
{
    public interface IDataSeeder
    {
        int Order { get; }

        Task<bool> ShouldSeedAsync(CancellationToken cancellationToken = default);
        Task<SeederResult> SeedAsync(CancellationToken cancellationToken = default);
    }

    public record SeederResult(
        string SeederName,
        int ItemsCreatedCount,
        IEnumerable<string> ItemsCreated
    );
}
