using API.Application.Features.Authentication.Services;
using API.Application.Features.Identity.Roles.Commands.Create;
using API.Application.Features.Identity.Roles.Commands.Delete;
using API.Application.Features.Identity.Roles.Commands.Restore;
using API.Application.Features.Identity.Roles.Commands.Update;
using API.Application.Features.Identity.Roles.Queries.GetAll;
using API.Application.Features.Identity.Roles.Queries.GetById;
using API.Contract.DTOs.Identity.Roles;
using API.Domain.Common.Constants;
using IdentityAPI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IdentityAPI.Controllers.Identity
{
    [Authorize(Roles = $"{DefaultRoles.SuperAdmin},{DefaultRoles.Admin},{DefaultRoles.Manager}")]
    [Route("api/identity/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public RolesController(
            IMediator mediator,
            ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] GetAllRolesQuery query,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return this.ToActionResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var query = new GetRoleByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);
            return this.ToActionResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateRoleRequest body,
            CancellationToken cancellationToken)
        {
            var command = new CreateRoleCommand(body);
            var result = await _mediator.Send(command, cancellationToken);
            return this.ToActionResult(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            [FromRoute] Guid id,
            [FromBody] UpdateRoleRequest body,
            CancellationToken cancellationToken)
        {
            var command = new UpdateRoleCommand(id, body);
            var result = await _mediator.Send(command, cancellationToken);
            return this.ToActionResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Detele(
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeleteRoleCommand(id);
            var result = await _mediator.Send(command, cancellationToken);
            return this.ToActionResult(result);
        }

        [HttpPatch("{id}/restore")]
        public async Task<IActionResult> Restore(
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var command = new RestoreRoleCommand(id);
            var result = await _mediator.Send(command, cancellationToken);
            return this.ToActionResult(result);
        }
    }
}
