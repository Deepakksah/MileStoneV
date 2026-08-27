using System;

namespace VideoOS.Platform.SDK.Platform;

public class ChangePasswordMIPException : MIPException
{
	public ChangePasswordMIPException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
