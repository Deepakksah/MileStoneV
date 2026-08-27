using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace VideoOS.IdentityServer.Model.Responses;

public class ErrorResponse : ProblemDetails, IErrorResponse
{
	private IReadOnlyDictionary<string, string[]> _errors;

	public IReadOnlyDictionary<string, string[]> Errors
	{
		get
		{
			return _errors;
		}
		set
		{
			if (value == null)
			{
				_errors = null;
				return;
			}
			_errors = value.Where((KeyValuePair<string, string[]> a) => ApiCodes.BlackListedCodes.All((ApiError c) => c.Code != a.Key)).ToImmutableDictionary();
		}
	}

	public ErrorResponse()
	{
	}

	public ErrorResponse(IEnumerable<IdentityError> errors)
		: this((from e in errors?.ToList()
			select new ApiError(e)).ToList())
	{
		if (errors == null)
		{
			throw new ArgumentNullException("errors");
		}
	}

	public ErrorResponse(ApiError error)
		: this(new ApiError[1] { error })
	{
		if (error == null)
		{
			throw new ArgumentNullException("error");
		}
	}

	public ErrorResponse(IEnumerable<ApiError> errors)
	{
		if (errors == null)
		{
			throw new ArgumentNullException("errors");
		}
		Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
		foreach (ApiError error in errors)
		{
			if (error.Code != null && dictionary.ContainsKey(error.Code))
			{
				dictionary[error.Code].Add(error.Description);
				continue;
			}
			dictionary.Add(error.Code ?? string.Empty, new List<string> { error.Description });
		}
		Errors = dictionary.Select((KeyValuePair<string, List<string>> a) => new KeyValuePair<string, string[]>(a.Key, a.Value.ToArray())).ToImmutableDictionary();
	}

	public static ErrorResponse FromInvalidModelState(ModelStateDictionary modelState)
	{
		ErrorResponse errorResponse = new ErrorResponse
		{
			Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
			Title = "One or more validation errors occurred.",
			Status = 400,
			Errors = modelState.Where<KeyValuePair<string, ModelStateEntry>>((KeyValuePair<string, ModelStateEntry> e) => e.Value.ValidationState == ModelValidationState.Invalid).ToDictionary((KeyValuePair<string, ModelStateEntry> k) => k.Key, (KeyValuePair<string, ModelStateEntry> v) => v.Value.Errors.Select((ModelError x) => x.ErrorMessage).ToArray())
		};
		errorResponse.Extensions.Add("reason", "ModelValidationError");
		return errorResponse;
	}
}
