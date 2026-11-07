using API.Contract.DTOs.Identity.Roles;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Identity.Roles.Commands.Update
{
    public record UpdateRoleCommand(Guid Id, UpdateRoleRequest Body)
        : IRequest<Result<RoleResponse>>
    {
    }
}

