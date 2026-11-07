using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Identity.Users.Commands.ToggleLock
{
    internal class ToggleLockUserCommandHandler(IUserService userService)
        : IRequestHandler<ToggleLockUserCommand, APIResponse>
    {
        private readonly IUserService _userService = userService;

        public async Task<APIResponse> Handle(
            ToggleLockUserCommand request,
            CancellationToken cancellationToken)
        {
            return await _userService.ToggleLockAsync(request, cancellationToken);
        }
    }
}

