using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Authentication.Commands.Logout
{
    internal class LogoutCommandHandler(IAuthenticationService authenticationService)
        : IRequestHandler<LogoutCommand, Result>
    {
        private readonly IAuthenticationService _authenticationService = authenticationService;

        public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            return await _authenticationService.LogoutAsync(request, cancellationToken);
        }
    }
}
