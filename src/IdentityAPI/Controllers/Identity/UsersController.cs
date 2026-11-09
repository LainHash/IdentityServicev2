using API.Application.Features.Authentication.Services;
using API.Application.Features.Identity.PersonalProfiles.Commands.UpdateByOwner;
using API.Application.Features.Identity.Roles.Queries.GetAll;
using API.Application.Features.Identity.Users.Commands.ChangePassword;
using API.Application.Features.Identity.Users.Commands.Delete;
using API.Application.Features.Identity.Users.Commands.ForceChangePassword;
using API.Application.Features.Identity.Users.Commands.Restore;
using API.Application.Features.Identity.Users.Commands.ToggleActive;
using API.Application.Features.Identity.Users.Commands.ToggleLock;
using API.Application.Features.Identity.Users.Queries.GetAll;
using API.Application.Features.Identity.Users.Queries.GetById;
using API.Contract.DTOs.Identity.PersonalProfiles;
using API.Contract.DTOs.Identity.Users;
using API.Domain.Common.Constants;
using IdentityAPI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IdentityAPI.Controllers.Identity
{
    [Route("api/identity/[controller]")]
    [ApiController]
    public class UsersController(
        IMediator mediator,
        ICurrentUserService currentUserService) : 
        ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser(
            CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            var query = new GetUserByIdQuery(userId!.Value);
            var result = await _mediator.Send(query, cancellationToken);
            return this.ToActionResult(result);
        }

        [Authorize]
        [HttpPut("me/profile")]
        public async Task<IActionResult> UpdateProfile(
            [FromBody] UpdatePersonalProfileRequest body,
            CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            var command = new UpdatePersonalProfileCommand(userId!.Value, body);
            var result = await _mediator.Send(command, cancellationToken);
            return this.ToActionResult(result);
        }

        [Authorize]
        [HttpPatch("me/change-password")]
        public async Task<IActionResult> ChangePassword(
            [FromBody] ChangePasswordRequest body,
            CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            var command = new ChangePasswordCommand(userId!.Value, body);
            var result = await _mediator.Send(command, cancellationToken);
            return this.ToActionResult(result);
        }


        [Authorize(Roles = $"{DefaultRoles.SuperAdmin},{DefaultRoles.Admin},{DefaultRoles.Manager}")]
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] GetAllUsersQuery query,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return this.ToActionResult(result);
        }

        [Authorize(Roles = $"{DefaultRoles.SuperAdmin},{DefaultRoles.Admin},{DefaultRoles.Manager}")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var query = new GetUserByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);
            return this.ToActionResult(result);
        }

        [Authorize(Roles = $"{DefaultRoles.SuperAdmin},{DefaultRoles.Admin}")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeleteUserCommand(id);
            var result = await _mediator.Send(command, cancellationToken);
            return this.ToActionResult(result);
        }

        [Authorize(Roles = $"{DefaultRoles.SuperAdmin},{DefaultRoles.Admin}")]
        [HttpPatch("{id:guid}/restore")]
        public async Task<IActionResult> Restore(
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var command = new RestoreUserCommand(id);
            var result = await _mediator.Send(command, cancellationToken);
            return this.ToActionResult(result);
        }

        [Authorize(Roles = $"{DefaultRoles.SuperAdmin},{DefaultRoles.Admin}")]
        [HttpPost("{id}/toggle-active")]
        public async Task<IActionResult> ToggleActive(
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var command = new ToggleActiveUserCommand(id);
            var result = await _mediator.Send(command, cancellationToken);
            return this.ToActionResult(result);
        }

        [Authorize(Roles = $"{DefaultRoles.SuperAdmin},{DefaultRoles.Admin}")]
        [HttpPost("{id}/toggle-lock")]
        public async Task<IActionResult> ToggleLock(
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var command = new ToggleLockUserCommand(id);
            var result = await _mediator.Send(command, cancellationToken);
            return this.ToActionResult(result);
        }

        [Authorize(Roles = $"{DefaultRoles.SuperAdmin},{DefaultRoles.Admin}")]
        [HttpPost("{id}/force-change-password")]
        public async Task<IActionResult> ForceChangePassword(
            [FromRoute] Guid id,
            [FromBody] string newPassword,
            CancellationToken cancellationToken)
        {
            var command = new ForceChangePasswordCommand(id, newPassword);
            var result = await _mediator.Send(command, cancellationToken);
            return this.ToActionResult(result);
        }
    }
}
