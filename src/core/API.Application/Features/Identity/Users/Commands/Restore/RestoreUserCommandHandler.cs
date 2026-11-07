using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Identity.Users.Commands.Restore
{
    internal class RestoreUserCommandHandler(IUserService userService)
                : IRequestHandler<RestoreUserCommand, APIResponse>
    {
        private readonly IUserService _userService = userService;

        public async Task<APIResponse> Handle(RestoreUserCommand request, CancellationToken cancellationToken)
        {
            return await _userService.RestoreAsync(request, cancellationToken);
        }
    }
}
