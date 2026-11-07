using API.Contract.DTOs.Authentication;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Authentication.Commands.GoogleLogin
{
    internal class GoogleLoginCommandHandler(IAuthenticationService authenticationService)
        : IRequestHandler<GoogleLoginCommand, APIResponse<AuthenticationResponse>>
    {
        private readonly IAuthenticationService _authenticationService = authenticationService;

        public async Task<APIResponse<AuthenticationResponse>> Handle(
            GoogleLoginCommand request,
            CancellationToken cancellationToken)
        {
            return await _authenticationService.GoogleLoginAsync(request, cancellationToken);
        }
    }
}
