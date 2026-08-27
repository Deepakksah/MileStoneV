using System;

namespace VideoOS.Installer.Logging.ErrorHandling;

public class LoggingException : Exception
{
	public LoggingException(string message)
		: base(message)
	{
	}

	public LoggingException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
