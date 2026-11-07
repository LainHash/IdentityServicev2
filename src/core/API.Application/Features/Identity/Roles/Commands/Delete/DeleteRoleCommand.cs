using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Identity.Roles.Commands.Delete
{
    public record DeleteRoleCommand(Guid Id)
        : IRequest<APIResponse>
    {
    }
}

