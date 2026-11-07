using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Identity.Users.Commands.ToggleActive
{
    public record ToggleActiveUserCommand(Guid Id)
        : IRequest<Result>
    {
    }
}

