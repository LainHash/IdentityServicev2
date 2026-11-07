using API.Contract.DTOs.Identity.Users;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Identity.Users.Queries.GetAll
{
    internal class GetAllUsersQueryHandler(IUserService userService)
                : IRequestHandler<GetAllUsersQuery, PaginationResponse<IEnumerable<UserResponse>>>
    {
        private readonly IUserService _userService = userService;

        public async Task<PaginationResponse<IEnumerable<UserResponse>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var specification = new GetAllUsersSpecification(request);
            return await _userService.GetAllAsync(specification, cancellationToken);
        }
    }
}
