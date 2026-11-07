using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Identity.Roles.Commands.Restore
{
    public record RestoreRoleCommand(Guid Id)
        : IRequest<Result>
    {
    }
}

