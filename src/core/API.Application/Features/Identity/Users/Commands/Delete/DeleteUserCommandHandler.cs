using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Identity.Users.Commands.Delete
{
    internal class DeleteUserCommandHandler(IUserService userService)
                : IRequestHandler<DeleteUserCommand, APIResponse>
    {
        private readonly IUserService _userService = userService;

        public async Task<APIResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            return await _userService.DeleteAsync(request, cancellationToken);
        }
    }
}
