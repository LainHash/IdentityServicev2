using API.Application.Features.Business.Import;
using API.Contract.DTOs.Business.Import;
using API.Domain.Common.Models.Results;
using MiniExcelLibs;
using System.Net;

namespace API.Infrastructure.Features.Business.Import
{
    internal class ImportService : IImportService
    {
        private readonly IEnumerable<ISheetImporter> _importers;

        public ImportService(IEnumerable<ISheetImporter> importers)
        {
            _importers = importers;
        }

        public async Task<Result<ImportResultResponse>> ImportAsync(
            Stream fileStream,
            string? sheetName = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(sheetName))
            {
                sheetName = DetectSheetName(fileStream);
            }

            if (string.IsNullOrWhiteSpace(sheetName))
            {
                var supported = string.Join(", ", _importers.Select(x => $"'{x.SheetName}'"));
                return Result<ImportResultResponse>.Fail(
                    $"No matching sheet found in the uploaded file. Supported sheets: {supported}.",
                    HttpStatusCode.BadRequest);
            }

            var cleanSheetName = sheetName.Replace(" ", "");
            var importer = _importers.FirstOrDefault(x =>
                x.SheetName.Equals(sheetName, StringComparison.OrdinalIgnoreCase) ||
                x.SheetName.Replace(" ", "").Equals(cleanSheetName, StringComparison.OrdinalIgnoreCase));

            if (importer is null)
            {
                var supported = string.Join(", ", _importers.Select(x => $"'{x.SheetName}'"));
                return Result<ImportResultResponse>.Fail(
                    $"Sheet '{sheetName}' is not supported. Supported sheets: {supported}.",
                    HttpStatusCode.BadRequest);
            }

            return await importer.ImportAsync(fileStream, cancellationToken);
        }

        private string? DetectSheetName(Stream stream)
        {
            try
            {
                var sheetNames = stream.GetSheetNames().ToList();
                stream.Position = 0;

                return sheetNames.FirstOrDefault(sheet =>
                {
                    var cleanSheet = sheet.Replace(" ", "");
                    return _importers.Any(i =>
                        i.SheetName.Equals(sheet, StringComparison.OrdinalIgnoreCase) ||
                        i.SheetName.Replace(" ", "").Equals(cleanSheet, StringComparison.OrdinalIgnoreCase));
                });
            }
            catch
            {
                stream.Position = 0;
                return null;
            }
        }
    }
}
