using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ms.MainApi.Core.Exceptions;
using ms.MainApi.Core.Models;

namespace ms.MainApi.Business.Pipelines;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IValidator<TRequest>? _validator;
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;

    public ValidationBehavior(ILogger<ValidationBehavior<TRequest, TResponse>> logger, IValidator<TRequest>? validator = null)
    {
        _validator = validator;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if(_validator is not null)
        {
            var context = new ValidationContext<TRequest>(request);
            var result = await _validator.ValidateAsync(request, cancellationToken);

            if(!result.IsValid)
            {
                var validationErrors = result.Errors.Select(failure => 
                    new ErrorModel("mainApi", nameof(ValidationErrorsExcception), failure.ErrorMessage, failure.ErrorMessage)).ToList();

                _logger.LogError("Validation failed: {ValidationErrors}", validationErrors);
            }
        }

        return await next();
    }
}
