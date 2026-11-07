using API.Contract.DTOs.Identity.Users;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Identity.Users.Queries.GetById
{
    internal class GetUserByIdQueryHandler(IUserService userService)
                : IRequestHandler<GetUserByIdQuery, APIResponse<UserResponse>>
    {
        private readonly IUserService _userService = userService;

        public async Task<APIResponse<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var specification = new GetUserByIdSpecification(request);
            return await _userService.GetByIdAsync(specification, cancellationToken);
        }
    }
}
