using System;
using System.Runtime.Serialization;

namespace VideoOS.Server;

[Serializable]
public class ServerConfigInvalidException : Exception
{
	public ServerConfigInvalidException()
	{
	}

	public ServerConfigInvalidException(string message)
		: base(message)
	{
	}

	public ServerConfigInvalidException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected ServerConfigInvalidException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
