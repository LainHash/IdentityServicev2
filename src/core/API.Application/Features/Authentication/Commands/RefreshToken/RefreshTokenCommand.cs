using API.Contract.DTOs.Authentication;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Authentication.Commands.RefreshToken
{
    public record RefreshTokenCommand(RefreshTokenRequest Body)
        : IRequest<APIResponse<AuthenticationResponse>>;
}
