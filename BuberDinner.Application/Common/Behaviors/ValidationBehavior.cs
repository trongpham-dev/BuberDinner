﻿using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Common;
using ErrorOr;
using FluentValidation;
using MediatR;

namespace BuberDinner.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
        where TRequest : IRequest<TResponse>
        where TResponse: IErrorOr
    {
        // because we might have 0 -> 1 validator so we put nullable ?
        private readonly IValidator<TRequest>? _validator;

        public ValidationBehavior(IValidator<TRequest>? validator = null)
        {
            _validator = validator;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if(_validator is null)
            {
                return await next();
            }
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (validationResult.IsValid)
            {
                return await next();
            }

            var errors = validationResult.Errors
                .Select(validationFailuer => Error.Validation(validationFailuer.PropertyName, validationFailuer.ErrorMessage)).ToList();

            return (dynamic)errors;
        }
    }
}
