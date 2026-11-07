using API.Contract.DTOs.Business.Import;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Business.Import.Commands.ImportExcel
{
    public record ImportExcelCommand(
        Stream FileStream,
        string? SheetName = null)
        : IRequest<Result<ImportResultResponse>>;
}
