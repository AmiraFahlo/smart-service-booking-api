using FluentValidation;

namespace SmartServiceBooking.Application.Common.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
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
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(r => r.Errors.Count != 0)
            .SelectMany(r => r.Errors)
            .Select(f => new Error(
                f.PropertyName, 
                f.ErrorMessage, 
                ErrorType.Validation))
            .ToArray();

        if (failures.Length != 0)
        {
            return CreateValidationResult<TResponse>(failures);
        }

        return await next();
    }

    private static TResponse CreateValidationResult<TResult>(Error[] failures)
    {
        var validationError = new ValidationError(failures);

        if (typeof(TResult) == typeof(Result))
        {
            return (TResponse)(object)Result.Failure(validationError);
        }

        if (typeof(TResult).IsGenericType && typeof(TResult).GetGenericTypeDefinition() == typeof(Result<>))
        {
            var valueType = typeof(TResult).GetGenericArguments()[0];
            var failureMethod = typeof(Result)
                .GetMethods()
                .First(m => m.Name == nameof(Result.Failure) && m.IsGenericMethod)
                .MakeGenericMethod(valueType);

            return (TResponse)failureMethod.Invoke(null, new object[] { validationError })!;
        }

        throw new ValidationException(failures.Select(f => new FluentValidation.Results.ValidationFailure(f.Code, f.Description)));
    }
}
