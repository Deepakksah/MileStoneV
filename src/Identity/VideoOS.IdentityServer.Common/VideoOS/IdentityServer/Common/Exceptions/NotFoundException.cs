using VideoOS.IdentityServer.Common.Model.Responses;

namespace VideoOS.IdentityServer.Common.Exceptions;

public class NotFoundException : IdpException
{
	public NotFoundException()
		: base(404, null, null)
	{
	}

	public NotFoundException(ErrorResponse errors)
		: base(404, errors?.Errors, errors?.Error)
	{
	}
}
