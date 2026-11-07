using API.Contract.DTOs.Business.Bootstrap;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Business.Bootstrap.Commands.Seed
{
    public record SeedBootstrapCommand(string? ProvidedKey) : IRequest<Result<BootstrapResponse>>;
}
