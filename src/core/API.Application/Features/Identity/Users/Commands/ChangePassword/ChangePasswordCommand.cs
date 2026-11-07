using API.Contract.DTOs.Identity.Users;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Identity.Users.Commands.ChangePassword
{
    public record ChangePasswordCommand(Guid UserId, ChangePasswordRequest Body)
        : IRequest<APIResponse>
    {
    }
}
