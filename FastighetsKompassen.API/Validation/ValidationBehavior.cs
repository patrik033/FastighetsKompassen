using FluentValidation;
using MediatR;
using System;

namespace FastighetsKompassen.API.Validation
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
       where TRequest : notnull
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
            // Create a validation context for the request
            var context = new ValidationContext<TRequest>(request);

            // Perform validations
            var validationFailures = await Task.WhenAll(
                _validators.Select(validator => validator.ValidateAsync(context, cancellationToken))
            );

            // Gather all validation errors
            var errors = validationFailures
                .Where(validationResult => !validationResult.IsValid)
                .SelectMany(validationResult => validationResult.Errors)
                .Select(validationFailure => new ValidationError(
                    validationFailure.PropertyName,
                    validationFailure.ErrorMessage))
                .ToList();

            // If there are any validation errors, throw a ValidationException
            if (errors.Any())
            {
                throw new ValidationException(errors);
            }

            // Proceed to the next pipeline behavior or handler
            return await next();
        }
    }

}
