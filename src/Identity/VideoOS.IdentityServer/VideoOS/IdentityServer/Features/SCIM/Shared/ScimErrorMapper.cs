using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using VideoOS.IdentityServer.Model.Result;

namespace VideoOS.IdentityServer.Features.SCIM.Shared;

public static class ScimErrorMapper
{
	public static Microsoft.AspNetCore.Http.IResult ToScimErrorResult(IReadOnlyList<Error> errors)
	{
		if (errors == null || errors.Count == 0)
		{
			throw new ArgumentException("The errors list cannot be null or empty.");
		}
		Error error = errors[0];
		ScimError.ScimErrorDefinition error2 = ScimError.GetError(error);
		ScimErrorResponse scimErrorResponse = new ScimErrorResponse
		{
			Detail = error.Description,
			Status = error2.StatusCode.ToString(),
			ScimType = error2.Type
		};
		return error2.StatusCode switch
		{
			409 => Results.Conflict(scimErrorResponse), 
			404 => Results.NotFound(scimErrorResponse), 
			403 => Results.Json(scimErrorResponse, (JsonSerializerOptions?)null, (string?)null, (int?)error2.StatusCode), 
			_ => Results.BadRequest(scimErrorResponse), 
		};
	}

	public static Microsoft.AspNetCore.Http.IResult ToScimErrorResult(Error error)
	{
		return ToScimErrorResult(new List<Error> { error });
	}
}
