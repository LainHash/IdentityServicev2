using API.Contract.DTOs.Identity.Roles;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Identity.Roles.Queries.GetById
{
    public record GetRoleByIdQuery(Guid Id)
        : IRequest<APIResponse<RoleDetailResponse>>
    {
    }
}

