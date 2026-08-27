using System;

namespace VideoOS.GisMapCommunication.Exceptions;

public class SecurityModeNotSupportedException : Exception
{
	public SecurityModeNotSupportedException()
	{
	}

	public SecurityModeNotSupportedException(string message)
		: base(message)
	{
	}

	public SecurityModeNotSupportedException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
