using VideoOS.IdentityServer.Common.Model.Responses;

namespace VideoOS.IdentityServer.Common.Exceptions;

public class UserNotInRoleException : IdpException
{
	public UserNotInRoleException(int statusCode, ErrorResponse errors)
		: base(statusCode, errors?.Errors, errors?.Error)
	{
	}
}
