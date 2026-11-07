using API.Domain.Common.Models.Results;
using FluentValidation;
using MediatR;
using System.Net;

namespace API.Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!_validators.Any())
                return await next(cancellationToken);

            var context = new ValidationContext<TRequest>(request);

            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count == 0)
                return await next(cancellationToken);

            var message = string.Join(" | ", failures.Select(f => f.ErrorMessage));

            var responseType = typeof(TResponse);

            if (responseType.IsGenericType &&
                responseType.GetGenericTypeDefinition() == typeof(Result<>))
            {
                var failMethod = responseType.GetMethod(
                    nameof(Result<>.Fail),
                    [typeof(string), typeof(HttpStatusCode)]);

                if (failMethod != null)
                    return (TResponse)failMethod.Invoke(null, [message, HttpStatusCode.UnprocessableEntity])!;
            }

            if (typeof(Result<>).IsAssignableFrom(responseType))
                return (TResponse)(object)Result<object>
                    .Fail(message, HttpStatusCode.UnprocessableEntity);

            throw new ValidationException(failures);
        }
    }
}
