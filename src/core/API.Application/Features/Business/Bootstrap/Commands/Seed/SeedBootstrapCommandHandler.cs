using API.Application.Features.Business.Bootstrap.Services;
using API.Contract.DTOs.Business.Bootstrap;
using API.Domain.Common.Models.Results;
using MediatR;

namespace API.Application.Features.Business.Bootstrap.Commands.Seed
{
    internal class SeedBootstrapCommandHandler(IBootstrapService bootstrapService)
        : IRequestHandler<SeedBootstrapCommand, Result<BootstrapResponse>>
    {
        private readonly IBootstrapService _bootstrapService = bootstrapService;

        public async Task<Result<BootstrapResponse>> Handle(SeedBootstrapCommand request, CancellationToken cancellationToken)
        {
            return await _bootstrapService.SeedAsync(request.ProvidedKey, cancellationToken);
        }
    }
}
