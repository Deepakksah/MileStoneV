using System;

namespace VideoOS.Common.Integration;

public class StatusException : Exception
{
	public StatusException(string message)
		: base(message)
	{
	}
}
