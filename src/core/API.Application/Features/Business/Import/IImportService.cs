using API.Contract.DTOs.Business.Import;
using API.Domain.Common.Models.Results;

namespace API.Application.Features.Business.Import
{
    public interface IImportService
    {
        Task<Result<ImportResultResponse>> ImportAsync(
            Stream fileStream,
            string? sheetName = null,
            CancellationToken cancellationToken = default);
    }
}
