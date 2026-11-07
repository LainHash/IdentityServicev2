using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Identity.Users.Commands.Delete
{
    public record DeleteUserCommand(Guid Id)
        : IRequest<APIResponse>
    {
    }
}
