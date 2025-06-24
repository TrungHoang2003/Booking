using BuildingBlocks.Commons;
using FluentValidation;
using MediatR;

namespace BuildingBlocks.PipelineBehaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, Result<TResponse>>
    where TRequest : IRequest<Result<TResponse>>
{
    public async Task<Result<TResponse>> Handle(TRequest request, RequestHandlerDelegate<Result<TResponse>> next, CancellationToken cancellationToken)
    {
        if (!validators.Any()) return await next(cancellationToken);
        
        var context = new ValidationContext<TRequest>(request);
        var failures = validators
            .Select(v => v.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(f => f is not null)
            .ToList();

        if (failures.Count == 0) return await next(cancellationToken);
        {
            var errorMessages = string.Join("; ", failures.Select(f => f.ErrorMessage));
            throw new Exception("Validation failed: " + errorMessages);
        }
    }
}