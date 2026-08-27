using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VideoOS.IdentityServer.Model.Result;

public static class ResultExtensions
{
	public static Microsoft.AspNetCore.Http.IResult ToLegacyErrorResponse(this Result result)
	{
		if (result.IsError)
		{
			IReadOnlyList<Error> errors = result.Errors;
			if (errors != null && errors.Any())
			{
				List<Error> source = result.Errors.Where((Error e) => e.Type == ErrorType.LegacyModelStateValidation).ToList();
				if (source.Any())
				{
					Dictionary<string, string[]> value = (from e in source
						group e by e.Code).ToDictionary((IGrouping<string, Error> g) => g.Key, (IGrouping<string, Error> g) => g.Select((Error e) => e.Description).ToArray());
					return Results.Problem(null, null, 400, "One or more validation errors occurred.", "https://tools.ietf.org/html/rfc7231#section-6.5.1", new Dictionary<string, object>
					{
						["errors"] = value,
						["reason"] = "ModelValidationError"
					});
				}
				Error primaryError = result.Errors.First();
				Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
				foreach (Error item in result.Errors.Where((Error e) => e.Type == primaryError.Type))
				{
					string text = item.Code;
					if (item.Code == "ApiException")
					{
						text = string.Empty;
					}
					if (text != null && dictionary.ContainsKey(text))
					{
						dictionary[text].Add(item.Description);
						continue;
					}
					string key = text ?? string.Empty;
					int num = 1;
					List<string> list = new List<string>(num);
					CollectionsMarshal.SetCount(list, num);
					Span<string> span = CollectionsMarshal.AsSpan(list);
					int num2 = 0;
					span[num2] = item.Description;
					num2++;
					dictionary.Add(key, list);
				}
				ImmutableDictionary<string, string[]> value2 = dictionary.Select((KeyValuePair<string, List<string>> a) => new KeyValuePair<string, string[]>(a.Key, a.Value.ToArray())).ToImmutableDictionary();
				int statusCode = GetStatusCode(primaryError.Type);
				ProblemDetails problemDetails = new ProblemDetails
				{
					Status = statusCode
				};
				problemDetails.Extensions.Add("errors", value2);
				problemDetails.Extensions.Add("extensions", new Dictionary<string, object>());
				return Results.Problem(problemDetails);
			}
		}
		throw new InvalidOperationException("Result is not an error");
	}

	private static int GetStatusCode(ErrorType errorType)
	{
		return errorType switch
		{
			ErrorType.Conflict => 409, 
			ErrorType.NotFound => 404, 
			ErrorType.Forbidden => 403, 
			_ => 400, 
		};
	}
}
