using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using VideoOS.IdentityServer.Model.Result;

namespace VideoOS.IdentityServer.Behaviours;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : IResult
{
	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		if (!validators.Any())
		{
			return await next();
		}
		ValidationContext<TRequest> context = new ValidationContext<TRequest>(request);
		List<ValidationResult> list = validators.Select((IValidator<TRequest> v) => v.Validate(context)).ToList();
		List<Error> list2 = new List<Error>();
		List<ValidationFailure> source = (from f in list.SelectMany((ValidationResult r) => r.Errors)
			where f != null
			select f).ToList();
		if (source.Any())
		{
			foreach (ValidationResult item2 in list)
			{
				foreach (ValidationFailure error in item2.Errors)
				{
					if (error.CustomState is Error item)
					{
						list2.Add(item);
					}
					else
					{
						list2.Add(Error.Validation(error.ErrorCode, error.ErrorMessage));
					}
				}
			}
		}
		if (!list2.Any())
		{
			return await next();
		}
		return (TResponse)(typeof(TResponse).GetMethod("From", BindingFlags.Static | BindingFlags.Public, new Type[1] { typeof(List<Error>) })?.Invoke(null, new object[1] { list2 }));
	}
}
