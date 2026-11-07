using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Identity.Users.Commands.ForceChangePassword
{
    public record ForceChangePasswordCommand(Guid Id, string NewPassword)
        : IRequest<APIResponse>
    {
    }
}
