namespace API.Contract.DTOs.Business.Bootstrap
{
    public record BootstrapResponse(
        string Message,
        int SettingsCreatedCount = 0,
        IEnumerable<string>? Settings = null,
        IEnumerable<SeederSummary>? Seeders = null
    );

    public record SeederSummary(
        string Name,
        int ItemsCreatedCount,
        IEnumerable<string> Items
    );
}
