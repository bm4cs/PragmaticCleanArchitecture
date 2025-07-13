using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Exceptions;
using FluentValidation;
using MediatR;

namespace Bookify.Application.Abstractions.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand // pipeline only applicable to command types, not queries
    where TResponse : notnull // ensure that the response type is not null, a requirement in MediatR handlers
{
    private readonly IEnumerable<IValidator<TRequest>> _validators; // all relevant validators that apply to the request type

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        if (!_validators.Any())
        {
            return await next(cancellationToken).ConfigureAwait(false);
        }

        var validationContext = new ValidationContext<TRequest>(request);

        var validationErrors = _validators
            .Select(v => v.Validate(validationContext))
            .Where(r => !r.IsValid)
            .SelectMany(r => r.Errors)
            .Select(validationError => new ValidationError(
                validationError.PropertyName,
                validationError.ErrorMessage
            ))
            .ToList();

        if (validationErrors.Count != 0)
        {
            throw new Exceptions.ValidationException(validationErrors);
        }

        return await next(cancellationToken).ConfigureAwait(false);
    }
}
