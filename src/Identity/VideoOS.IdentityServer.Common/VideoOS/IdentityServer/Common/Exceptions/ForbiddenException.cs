using VideoOS.IdentityServer.Common.Model.Responses;

namespace VideoOS.IdentityServer.Common.Exceptions;

public class ForbiddenException : IdpException
{
	public ForbiddenException(ErrorResponse errors)
		: base(403, errors?.Errors, errors?.Error)
	{
	}
}
