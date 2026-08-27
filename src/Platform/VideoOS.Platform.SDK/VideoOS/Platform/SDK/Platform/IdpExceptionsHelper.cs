using VideoOS.IdentityServer.Common.Exceptions;

namespace VideoOS.Platform.SDK.Platform;

public static class IdpExceptionsHelper
{
	public static MIPException ConvertToMIPException(string message, IdpException idpException)
	{
		if (idpException is UnauthorizedException)
		{
			return new InvalidCredentialsMIPException(message, idpException);
		}
		if (idpException is LockedOutException)
		{
			return new LockedOutMIPException(message, idpException);
		}
		if (idpException is ChangePasswordException)
		{
			return new ChangePasswordMIPException(message, idpException);
		}
		return new MIPException(message, idpException);
	}
}
