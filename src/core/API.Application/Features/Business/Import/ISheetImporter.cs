using API.Contract.DTOs.Business.Import;
using API.Domain.Common.Models.Results;

namespace API.Application.Features.Business.Import
{
    public interface ISheetImporter
    {
        string SheetName { get; }

        Task<Result<ImportResultResponse>> ImportAsync(Stream stream, CancellationToken cancellationToken = default);
    }
}
