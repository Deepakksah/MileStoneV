using VideoOS.IdentityServer.Common.Model.Responses;

namespace VideoOS.IdentityServer.Common.Exceptions;

public class InternalServerErrorException : IdpException
{
	public InternalServerErrorException(ErrorResponse errors)
		: base(500, errors?.Errors, errors?.Error)
	{
	}
}
