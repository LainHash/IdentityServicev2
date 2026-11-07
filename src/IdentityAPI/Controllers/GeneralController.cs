using API.Application.Features.Business.Bootstrap.Commands.Seed;
using API.Application.Features.Business.Import.Commands.ImportExcel;
using IdentityAPI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAPI.Controllers
{
    [ApiController]
    [Route("api/identity")]
    public class GeneralController(IMediator mediator)
        : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public IActionResult GetRoot()
        {
            return Ok(new
            {
                success = true,
                message = "API is running",
                version = "1.0.0"
            });
        }

        //[Authorize(Roles = "SuperAdmin,Admin,Manager")]
        [HttpPost("import")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Import(
            IFormFile file,
            [FromQuery] string? sheetName = null,
            CancellationToken cancellationToken = default)
        {
            var validationError = ValidateFile(file);
            if (validationError is not null)
            {
                return BadRequest(new { message = validationError });
            }

            await using var stream = file.OpenReadStream();
            var command = new ImportExcelCommand(stream, sheetName);
            var result = await _mediator.Send(command, cancellationToken);
            return this.ToActionResult(result);
        }

        private static string? ValidateFile(IFormFile? file)
        {
            if (file is null || file.Length == 0)
            {
                return "Please upload a valid Excel file.";
            }

            var allowedExtensions = new[] { ".xlsx", ".xls" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
            {
                return "Only .xlsx and .xls files are accepted.";
            }

            return null;
        }

        [AllowAnonymous]
        [HttpPost("bootstrap/seed")]
        public async Task<IActionResult> SeedAsync(
            [FromHeader(Name = "X-Bootstrap-Key")] string? bootstrapKey,
            CancellationToken cancellationToken = default)
        {
            var command = new SeedBootstrapCommand(bootstrapKey);
            var result = await _mediator.Send(command, cancellationToken);
            return this.ToActionResult(result);
        }
    }
}
