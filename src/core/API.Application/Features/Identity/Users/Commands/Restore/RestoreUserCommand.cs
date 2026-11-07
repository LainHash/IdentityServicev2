using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Identity.Users.Commands.Restore
{
    public record RestoreUserCommand(Guid Id)
        : IRequest<APIResponse>
    {
    }
}
