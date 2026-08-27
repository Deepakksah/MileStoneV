using VideoOS.IdentityServer.Common.Model.Responses;

namespace VideoOS.IdentityServer.Common.Exceptions;

public class PasswordNotComplexEnoughException : IdpException
{
	public PasswordNotComplexEnoughException(int statusCode, ErrorResponse errors)
		: base(statusCode, errors?.Errors, errors?.Error)
	{
	}
}
