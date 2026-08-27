using VideoOS.IdentityServer.Common.Model.Responses;

namespace VideoOS.IdentityServer.Common.Exceptions;

public class AdminIdentityNotFoundException : IdpException
{
	public AdminIdentityNotFoundException(int statusCode, ErrorResponse errors)
		: base(statusCode, errors?.Errors, errors?.Error)
	{
	}
}
