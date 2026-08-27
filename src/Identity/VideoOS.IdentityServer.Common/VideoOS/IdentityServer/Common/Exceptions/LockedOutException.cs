using VideoOS.IdentityServer.Common.Model.Responses;

namespace VideoOS.IdentityServer.Common.Exceptions;

public class LockedOutException : IdpException
{
	public LockedOutException(int statusCode, ErrorResponse errors)
		: base(statusCode, errors?.Errors, errors?.Error)
	{
	}
}
