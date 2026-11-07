using API.Contract.DTOs.Business.Bootstrap;
using API.Domain.Common.Models.Results;

namespace API.Application.Features.Business.Bootstrap.Services
{
    public interface IBootstrapService
    {
        Task<Result<BootstrapResponse>> SeedAsync(
            string? providedKey,
            CancellationToken cancellationToken = default);
    }
}
