using API.Contract.DTOs.Identity.Roles;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Identity.Roles.Commands.Create
{
    public record CreateRoleCommand(CreateRoleRequest Body)
        : IRequest<Result<RoleResponse>>
    {
    }
}

