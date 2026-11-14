using API.Application.Features.Personnel.Positions.Commands.Create;
using API.Application.Features.Personnel.Positions.Commands.Delete;
using API.Application.Features.Personnel.Positions.Commands.Restore;
using API.Application.Features.Personnel.Positions.Commands.Update;
using API.Application.Features.Personnel.Positions.Queries.GetAll;
using API.Application.Features.Personnel.Positions.Queries.GetById;
using API.Contract.DTOs.Personnel.Positions;
using API.Domain.Common.Constants;
using IdentityAPI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAPI.Controllers.Personnel
{
    [Authorize(Roles = $"{DefaultRoles.SuperAdmin},{DefaultRoles.Admin},{DefaultRoles.Manager}")]
    [Route("api/identity/[controller]")]
    [ApiController]
    public class PositionsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] GetAllPositionsQuery query,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return this.ToActionResult(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var query = new GetPositionByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);
            return this.ToActionResult(result);
        }

        [Authorize(Roles = $"{DefaultRoles.SuperAdmin},{DefaultRoles.Admin}")]
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreatePositionRequest body,
            CancellationToken cancellationToken)
        {
            var command = new CreatePositionCommand(body);
            var result = await _mediator.Send(command, cancellationToken);
            return this.ToActionResult(result);
        }

        [Authorize(Roles = $"{DefaultRoles.SuperAdmin},{DefaultRoles.Admin}")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            [FromRoute] Guid id,
            [FromBody] UpdatePositionRequest body,
            CancellationToken cancellationToken)
        {
            var command = new UpdatePositionCommand(id, body);
            var result = await _mediator.Send(command, cancellationToken);
            return this.ToActionResult(result);
        }

        [Authorize(Roles = $"{DefaultRoles.SuperAdmin},{DefaultRoles.Admin}")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeletePositionCommand(id);
            var result = await _mediator.Send(command, cancellationToken);
            return this.ToActionResult(result);
        }

        [Authorize(Roles = $"{DefaultRoles.SuperAdmin},{DefaultRoles.Admin}")]
        [HttpPatch("{id:guid}/restore")]
        public async Task<IActionResult> Restore(
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var command = new RestorePositionCommand(id);
            var result = await _mediator.Send(command, cancellationToken);
            return this.ToActionResult(result);
        }
    }
}

