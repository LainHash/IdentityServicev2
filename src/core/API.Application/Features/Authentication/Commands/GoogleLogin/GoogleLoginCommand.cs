using API.Contract.DTOs.Authentication;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Authentication.Commands.GoogleLogin
{
    public record GoogleLoginCommand(GoogleLoginRequest Body)
        : IRequest<Result<AuthenticationResponse>>
    {
    }
}
