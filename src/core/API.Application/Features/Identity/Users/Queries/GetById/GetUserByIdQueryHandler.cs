using API.Contract.DTOs.Identity.Users;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Identity.Users.Queries.GetById
{
    internal class GetUserByIdQueryHandler(IUserService userService)
                : IRequestHandler<GetUserByIdQuery, Result<UserDetailResponse>>
    {
        private readonly IUserService _userService = userService;

        public async Task<Result<UserDetailResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var specification = new GetUserByIdSpecification(request);
            return await _userService.GetByIdAsync(specification, cancellationToken);
        }
    }
}
