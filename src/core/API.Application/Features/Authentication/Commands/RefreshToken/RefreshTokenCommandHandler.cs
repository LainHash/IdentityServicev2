using API.Contract.DTOs.Authentication;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Authentication.Commands.RefreshToken
{
    internal class RefreshTokenCommandHandler(IAuthenticationService authenticationService)
        : IRequestHandler<RefreshTokenCommand, APIResponse<AuthenticationResponse>>
    {
        private readonly IAuthenticationService _authenticationService = authenticationService;

        public async Task<APIResponse<AuthenticationResponse>> Handle(
            RefreshTokenCommand request,
            CancellationToken cancellationToken)
        {
            return await _authenticationService.RefreshTokenAsync(request, cancellationToken);
        }
    }
}
