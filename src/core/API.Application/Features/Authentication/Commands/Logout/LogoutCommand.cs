using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Authentication.Commands.Logout
{
    public record LogoutCommand(string RefreshToken)
        : IRequest<APIResponse>;
}
