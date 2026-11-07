using API.Application.Features.Authentication;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Identity.Users.Commands.ChangePassword
{
    internal class ChangePasswordCommandHandler(IUserService userService)
                : IRequestHandler<ChangePasswordCommand, Result>
    {
        private readonly IUserService _userService = userService;

        public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            return await _userService.ChangePasswordAsync(request, cancellationToken);
        }
    }
}
