using VideoOS.IdentityServer.Common.Model.Responses;

namespace VideoOS.IdentityServer.Common.Exceptions;

public class BadArgumentsException : IdpException
{
	public BadArgumentsException(ErrorResponse errors)
		: base(400, errors?.Errors, errors?.Error)
	{
	}
}
