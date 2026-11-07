using API.Application.Features.Business;
using API.Application.Features.Business.Import;
using API.Contract.DTOs.Business.Import;
using API.Domain.Common.Models.Results;
using API.Infrastructure.Common.Helpers;
using System.Net;

namespace API.Infrastructure.Features.Business.Import
{
    public abstract class BaseSheetImporter<TRow> : ISheetImporter where TRow : class, new()
    {
        protected readonly IUnitOfWork UnitOfWork;

        protected BaseSheetImporter(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public abstract string SheetName { get; }

        public virtual async Task<Result<ImportResultResponse>> ImportAsync(
            Stream stream,
            CancellationToken cancellationToken = default)
        {
            var matchedSheet = ExcelHelper.FindMatchingSheet(stream, SheetName);
            if (matchedSheet is null)
            {
                return Result<ImportResultResponse>.Fail(
                    $"Sheet '{SheetName}' was not found in the uploaded file.",
                    HttpStatusCode.BadRequest);
            }

            var (rows, readError) = ExcelHelper.ReadRows<TRow>(stream, matchedSheet);
            if (readError is not null)
            {
                return Result<ImportResultResponse>.Fail(
                    $"Failed to read sheet '{SheetName}': {readError}",
                    HttpStatusCode.BadRequest);
            }

            var rowList = rows as IReadOnlyList<TRow> ?? rows.ToList();

            await BeforeProcessRowsAsync(rowList, cancellationToken);

            var result = new ImportResultResponse();
            var rowIndex = 2; // Row 1 is header, data starts at row 2

            foreach (var row in rowList)
            {
                result.TotalRows++;

                try
                {
                    await ProcessRowAsync(row, rowIndex, result, cancellationToken);
                }
                catch (Exception ex)
                {
                    result.AddError(rowIndex, ex.Message);
                }

                rowIndex++;
            }

            if (result.SuccessCount > 0)
            {
                await UnitOfWork.SaveChangesAsync(cancellationToken);
            }

            var message = $"Import completed for '{SheetName}': {result.SuccessCount} succeeded, {result.FailedCount} failed out of {result.TotalRows} rows.";
            return Result<ImportResultResponse>.Succeed(result, message);
        }

        protected virtual Task BeforeProcessRowsAsync(
            IReadOnlyList<TRow> rows,
            CancellationToken cancellationToken) => Task.CompletedTask;

        protected abstract Task ProcessRowAsync(
            TRow row,
            int rowIndex,
            ImportResultResponse result,
            CancellationToken cancellationToken);
    }
}
