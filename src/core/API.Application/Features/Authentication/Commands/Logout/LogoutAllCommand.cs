using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Authentication.Commands.Logout
{
    public record LogoutAllCommand(Guid UserId)
        : IRequest<Result>;
}
