using System;

namespace VideoOS.Utilities.Security;

public class VistaToolsException : ApplicationException
{
	public VistaToolsException()
	{
	}

	public VistaToolsException(string message)
		: base(message)
	{
	}

	public VistaToolsException(string message, Exception inner)
		: base(message, inner)
	{
	}
}
