using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Identity.Users.Commands.ForceChangePassword
{
    internal class ForceChangePasswordCommandHandler(IUserService userService)
                : IRequestHandler<ForceChangePasswordCommand, APIResponse>
    {
        private readonly IUserService _userService = userService;

        public async Task<APIResponse> Handle(ForceChangePasswordCommand request, CancellationToken cancellationToken)
        {
            return await _userService.ForceChangePasswordAsync(request, cancellationToken);
        }
    }
}
