using FluentValidation;
using MediatR;
using MyFinances.Application.Abstractions.Interfaces;

namespace MyFinances.Application.Abstractions.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommandBase
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var validationResults =
            await Task.WhenAll(_validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));
        var failures = validationResults
            .Where(result => !result.IsValid)
            .SelectMany(result => result.Errors)
            .ToList();

        if (failures.Count != 0)
            throw new ValidationException("One or more validation failures have occurred.", failures);

        return await next();
    }
}