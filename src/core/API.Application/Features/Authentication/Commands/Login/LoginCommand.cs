using API.Contract.DTOs.Authentication;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Authentication.Commands.Login
{
    public record LoginCommand(LoginRequest Body)
        : IRequest<Result<AuthenticationResponse>>
    {
    }
}
