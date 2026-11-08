using API.Application.Features.Business.Audit.Queries.GetAll;
using IdentityAPI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAPI.Controllers.Business.Audit
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    [Route("api/admin/identity/audit-logs")]
    [ApiController]
    public class AuditLogsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] GetAllAuditLogsQuery query,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return this.ToActionResult(result);
        }
    }
}
