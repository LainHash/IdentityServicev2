using API.Application.Features.Authentication;
using API.Application.Features.Business.Audit;
using MediatR;

namespace API.Application.Behaviors
{
    public class AuditLogBehavior<TRequest, TResponse> 
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IAuditContext _auditContext;
        private readonly ICurrentUserService _currentUser;

        public AuditLogBehavior(
            IAuditContext auditContext,
            ICurrentUserService currentUser)
        {
            _auditContext = auditContext;
            _currentUser = currentUser;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            _auditContext.UserPublicId = _currentUser.UserPublicId;
            _auditContext.IpAddress = "";

            return await next(cancellationToken);
        }
    }
}
