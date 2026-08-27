using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.Server.ConfigurationApi;

[Serializable]
public class PathInvalidException : Exception
{
	public PathInvalidException()
	{
	}

	public PathInvalidException(string message)
		: base(message)
	{
	}

	public PathInvalidException(string message, Exception exception)
		: base(message, exception)
	{
	}

	protected PathInvalidException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
