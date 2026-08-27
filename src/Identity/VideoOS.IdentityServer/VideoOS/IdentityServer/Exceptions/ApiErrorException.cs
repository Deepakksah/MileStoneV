using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using VideoOS.IdentityServer.Model;

namespace VideoOS.IdentityServer.Exceptions;

public class ApiErrorException : ApiException
{
	public ApiErrorException(IEnumerable<IdentityError> errors)
		: base(400, errors)
	{
	}

	public ApiErrorException(ApiError error)
		: base(400, error)
	{
	}

	public ApiErrorException(IEnumerable<ApiError> errors)
		: base(400, errors)
	{
	}
}
