using API.Application.Features.Personnel.Departments.Commands.Create;
using API.Application.Features.Personnel.Departments.Commands.Delete;
using API.Application.Features.Personnel.Departments.Commands.Restore;
using API.Application.Features.Personnel.Departments.Commands.Update;
using API.Application.Features.Personnel.Departments.Queries.GetAll;
using API.Application.Features.Personnel.Departments.Queries.GetById;
using API.Contract.DTOs.Personnel.Departments;
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
    public class DepartmentsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] GetAllDepartmentsQuery query,
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
            var query = new GetDepartmentByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);
            return this.ToActionResult(result);
        }

        [Authorize(Roles = $"{DefaultRoles.SuperAdmin},{DefaultRoles.Admin}")]
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateDepartmentRequest body,
            CancellationToken cancellationToken)
        {
            var command = new CreateDepartmentCommand(body);
            var result = await _mediator.Send(command, cancellationToken);
            return this.ToActionResult(result);
        }

        [Authorize(Roles = $"{DefaultRoles.SuperAdmin},{DefaultRoles.Admin}")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            [FromRoute] Guid id,
            [FromBody] UpdateDepartmentRequest body,
            CancellationToken cancellationToken)
        {
            var command = new UpdateDepartmentCommand(id, body);
            var result = await _mediator.Send(command, cancellationToken);
            return this.ToActionResult(result);
        }

        [Authorize(Roles = $"{DefaultRoles.SuperAdmin},{DefaultRoles.Admin}")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeleteDepartmentCommand(id);
            var result = await _mediator.Send(command, cancellationToken);
            return this.ToActionResult(result);
        }

        [Authorize(Roles = $"{DefaultRoles.SuperAdmin},{DefaultRoles.Admin}")]
        [HttpPatch("{id:guid}/restore")]
        public async Task<IActionResult> Restore(
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var command = new RestoreDepartmentCommand(id);
            var result = await _mediator.Send(command, cancellationToken);
            return this.ToActionResult(result);
        }
    }
}

