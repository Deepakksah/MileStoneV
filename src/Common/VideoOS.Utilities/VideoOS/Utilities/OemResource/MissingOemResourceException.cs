using System;

namespace VideoOS.Utilities.OemResource;

public class MissingOemResourceException : Exception
{
	public MissingOemResourceException()
	{
	}

	public MissingOemResourceException(string message)
		: base(message)
	{
	}

	public MissingOemResourceException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
