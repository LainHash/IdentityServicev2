using API.Contract.DTOs.Identity.Users;
using API.Domain.Common.Models;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Identity.Users.Queries.GetAll
{
    public record GetAllUsersQuery
        : PaginationQuery, IRequest<PaginationResult<IEnumerable<UserResponse>>>
    {
    }
}
