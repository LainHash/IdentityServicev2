using API.Contract.DTOs.Business.Import;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Business.Import.Commands.ImportExcel
{
    internal class ImportExcelCommandHandler(IImportService importService)
        : IRequestHandler<ImportExcelCommand, Result<ImportResultResponse>>
    {
        private readonly IImportService _importService = importService;

        public async Task<Result<ImportResultResponse>> Handle(
            ImportExcelCommand request,
            CancellationToken cancellationToken)
        {
            return await _importService
                .ImportAsync(request.FileStream, request.SheetName, cancellationToken);
        }
    }
}
