using API.Application.Features.Authentication.Services;
using API.Application.Features.Guest.Queries.GetAll;
using API.Application.Features.Guest.Queries.GetById;
using API.Application.Features.Guest.Queries.GetByUserId;
using API.Domain.Common.Constants;
using IdentityAPI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityAPI.Controllers.Guest
{
    [Route("api/identity/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public CustomersController(IMediator mediator, ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile(CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (!userId.HasValue)
            {
                return Unauthorized();
            }

            var query = new GetCustomerByUserIdQuery(userId.Value);
            var result = await _mediator.Send(query, cancellationToken);
            return this.ToActionResult(result);
        }

        [Authorize]
        [HttpGet("by-user/{userId:guid}")]
        public async Task<IActionResult> GetByUserId(
            [FromRoute] Guid userId,
            CancellationToken cancellationToken)
        {
            var query = new GetCustomerByUserIdQuery(userId);
            var result = await _mediator.Send(query, cancellationToken);
            return this.ToActionResult(result);
        }

        [Authorize(Roles = $"{DefaultRoles.SuperAdmin},{DefaultRoles.Admin},{DefaultRoles.Manager}")]
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] GetAllCustomersQuery query,
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
            var query = new GetCustomerByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);
            return this.ToActionResult(result);
        }
    }
}
