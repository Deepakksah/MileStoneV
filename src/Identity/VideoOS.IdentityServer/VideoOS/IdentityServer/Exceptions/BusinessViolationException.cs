using System;

namespace VideoOS.IdentityServer.Exceptions;

public sealed class BusinessViolationException : ApiException
{
	public BusinessViolationException(string message)
		: base(400, message)
	{
	}

	public BusinessViolationException(string message, Exception innerException)
		: base(400, message, innerException)
	{
	}
}
