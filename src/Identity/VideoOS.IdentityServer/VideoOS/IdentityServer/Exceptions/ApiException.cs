using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using VideoOS.IdentityServer.Model;
using VideoOS.IdentityServer.Model.Responses;

namespace VideoOS.IdentityServer.Exceptions;

public abstract class ApiException : Exception
{
	public ErrorResponse ErrorResponse { get; }

	public int StatusCode { get; private set; }

	protected ApiException(int statusCode, IEnumerable<IdentityError> errors)
		: this(statusCode, (string)null)
	{
		ErrorResponse = new ErrorResponse(errors);
	}

	protected ApiException(int statusCode, ApiError error)
		: this(statusCode, (string)null)
	{
		ErrorResponse = new ErrorResponse(error);
	}

	protected ApiException(int statusCode, IEnumerable<ApiError> errors)
		: this(statusCode, (string)null)
	{
		ErrorResponse = new ErrorResponse(errors);
	}

	protected ApiException(int statusCode, string message)
		: base(message)
	{
		SetStatusCode(statusCode);
	}

	protected ApiException(int statusCode, string message, Exception innerException)
		: base(message, innerException)
	{
		SetStatusCode(statusCode);
	}

	private void SetStatusCode(int statusCode)
	{
		if (statusCode <= 399 || statusCode > 599)
		{
			throw new ArgumentOutOfRangeException("statusCode", "Can only be invoked with a failure status code in the range 400-599.");
		}
		StatusCode = statusCode;
	}
}
