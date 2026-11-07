using API.Contract.DTOs.Identity.Roles;
using API.Domain.Common.Models;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Identity.Roles.Queries.GetAll
{
    public record GetAllRolesQuery
        : PaginationQuery, IRequest<PaginationResult<IEnumerable<RoleResponse>>>
    {
    }
}

