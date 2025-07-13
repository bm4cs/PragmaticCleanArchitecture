using Bookify.Application.Abstractions.Messaging;
using FluentValidation;
using MediatR;

namespace Bookify.Application.Abstractions.Behaviors;

public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand // only commands are validated, not queries
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

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        return await next();
    }
}
