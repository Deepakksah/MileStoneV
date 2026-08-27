using System;

namespace VideoOS.Utilities.OemResource;

public class InvalidOemResourceException : Exception
{
	public InvalidOemResourceException()
	{
	}

	public InvalidOemResourceException(string message)
		: base(message)
	{
	}

	public InvalidOemResourceException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
