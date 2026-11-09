using API.Contract.DTOs.Identity.Users;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Identity.Users.Queries.GetById
{
    public record GetUserByIdQuery(Guid Id)
        : IRequest<Result<UserDetailResponse>>
    {
    }
}
