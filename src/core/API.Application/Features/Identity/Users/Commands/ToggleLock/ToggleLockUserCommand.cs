using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Identity.Users.Commands.ToggleLock
{
    public record ToggleLockUserCommand(Guid Id)
        : IRequest<Result>
    {
    }
}

