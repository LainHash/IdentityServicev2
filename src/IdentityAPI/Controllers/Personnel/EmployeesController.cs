using API.Application.Features.Personnel.Employees.Queries.GetAll;
using IdentityAPI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAPI.Controllers.Personnel
{
    [Authorize(Roles = "SuperAdmin,Admin,Manager")]
    [Route("api/identity/[controller]")]
    [ApiController]
    public class EmployeesController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] GetAllEmployeesQuery query,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return this.ToActionResult(result);
        }
    }
}
