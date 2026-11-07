using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Identity.Users.Commands.ToggleLock
{
    internal class ToggleLockUserCommandHandler(IUserService userService)
        : IRequestHandler<ToggleLockUserCommand, Result>
    {
        private readonly IUserService _userService = userService;

        public async Task<Result> Handle(
            ToggleLockUserCommand request,
            CancellationToken cancellationToken)
        {
            return await _userService.ToggleLockAsync(request, cancellationToken);
        }
    }
}

