using System;

namespace VideoOS.DataCollector.Media;

public class NvidiaInitializationException : Exception
{
	public NvidiaInitializationException()
	{
	}

	public NvidiaInitializationException(string message)
		: base(message)
	{
	}

	public NvidiaInitializationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
