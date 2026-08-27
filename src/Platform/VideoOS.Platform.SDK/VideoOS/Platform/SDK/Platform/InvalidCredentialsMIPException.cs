using System;

namespace VideoOS.Platform.SDK.Platform;

public class InvalidCredentialsMIPException : NotAuthorizedMIPException
{
	public InvalidCredentialsMIPException(string host)
		: base($"Credentials are not valid for the server. Host name: {host}.")
	{
	}

	public InvalidCredentialsMIPException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
