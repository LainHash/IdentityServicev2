using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Authentication.Commands.Register
{
    internal class RegisterCommandHandler(IAuthenticationService authenticationService)
                : IRequestHandler<RegisterCommand, Result>
    {
        private readonly IAuthenticationService _authenticationService = authenticationService;

        public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var response = await _authenticationService.RegisterAsync(request, cancellationToken);
            return response;
        }
    }
}
