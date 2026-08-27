using System;

namespace VideoOS.Installer.Core.Cab;

public class CabException : Exception
{
	public CabException(string message)
		: base(message)
	{
	}

	public CabException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
