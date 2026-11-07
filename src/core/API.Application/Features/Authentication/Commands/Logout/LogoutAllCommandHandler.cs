using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Authentication.Commands.Logout
{
    internal class LogoutAllCommandHandler(IAuthenticationService authenticationService)
        : IRequestHandler<LogoutAllCommand, APIResponse>
    {
        private readonly IAuthenticationService _authenticationService = authenticationService;

        public async Task<APIResponse> Handle(LogoutAllCommand request, CancellationToken cancellationToken)
        {
            return await _authenticationService.LogoutAllAsync(request, cancellationToken);
        }
    }
}
