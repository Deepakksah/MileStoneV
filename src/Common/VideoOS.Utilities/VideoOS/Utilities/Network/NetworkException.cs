using System;

namespace VideoOS.Utilities.Network;

public class NetworkException : Exception
{
	public NetworkException()
	{
	}

	public NetworkException(string message)
		: base(message)
	{
	}

	public NetworkException(string message, Exception inner)
		: base(message, inner)
	{
	}
}
