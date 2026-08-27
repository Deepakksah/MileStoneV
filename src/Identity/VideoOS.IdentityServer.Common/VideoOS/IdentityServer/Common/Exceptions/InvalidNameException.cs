using VideoOS.IdentityServer.Common.Model.Responses;

namespace VideoOS.IdentityServer.Common.Exceptions;

public class InvalidNameException : IdpException
{
	public InvalidNameException(int statusCode, ErrorResponse errors)
		: base(statusCode, errors?.Errors, errors?.Error)
	{
	}
}
