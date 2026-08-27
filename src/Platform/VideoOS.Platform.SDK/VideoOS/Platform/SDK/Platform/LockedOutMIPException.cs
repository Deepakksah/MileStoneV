using System;

namespace VideoOS.Platform.SDK.Platform;

public class LockedOutMIPException : MIPException
{
	public LockedOutMIPException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
