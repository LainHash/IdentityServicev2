using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Identity.Users.Commands.ToggleActive
{
    internal class ToggleActiveUserCommandHandler(IUserService userService)
        : IRequestHandler<ToggleActiveUserCommand, Result>
    {
        private readonly IUserService _userService = userService;

        public async Task<Result> Handle(
            ToggleActiveUserCommand request,
            CancellationToken cancellationToken)
        {
            return await _userService.ToggleActiveAsync(request, cancellationToken);
        }
    }
}

