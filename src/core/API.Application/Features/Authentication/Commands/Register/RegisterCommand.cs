using API.Contract.DTOs.Authentication;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Authentication.Commands.Register
{
    public record RegisterCommand(RegisterRequest Body)
        : IRequest<APIResponse>
    {
    }
}
