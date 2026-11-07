using API.Contract.DTOs.Authentication;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Authentication.Commands.Login
{
    internal class LoginCommandHandler(IAuthenticationService authenticationService)
                : IRequestHandler<LoginCommand, APIResponse<AuthenticationResponse>>
    {
        private readonly IAuthenticationService _authenticationService = authenticationService;

        public async Task<APIResponse<AuthenticationResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var response = await _authenticationService.LoginAsync(request, cancellationToken);
            return response;
        }
    }
}
