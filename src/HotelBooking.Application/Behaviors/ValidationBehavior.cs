using FluentValidation;
using HotelBooking.Domain.Common;
using MediatR;

namespace HotelBooking.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
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
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var failures = _validators
            .Select(v => v.Validate(context))
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToList();

        if (failures.Count == 0)
            return await next();

        // Converts FluentValidation failures into our Result pattern.
        // Only works when TResponse is Result or Result<T>.
        var errors = failures
            .Select(f => DomainError.Validation(f.PropertyName, f.ErrorMessage))
            .ToList();

        var responseType = typeof(TResponse);

        if (responseType == typeof(Result))
            return (TResponse)(object)Result.Failure(errors.First());

        if (responseType.IsGenericType &&
            responseType.GetGenericTypeDefinition() == typeof(Result<>))
        {
            var resultType = responseType.GetGenericArguments()[0];
            var failureMethod = typeof(Result)
                .GetMethods()
                .First(m => m.Name == "Failure" && m.IsGenericMethod)
                .MakeGenericMethod(resultType);

            return (TResponse)failureMethod.Invoke(null, [errors.First()])!;
        }

        throw new ValidationException(failures);
    }
}
