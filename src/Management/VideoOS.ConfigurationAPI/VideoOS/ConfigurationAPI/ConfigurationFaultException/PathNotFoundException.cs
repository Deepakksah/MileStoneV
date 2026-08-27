using System;
using System.Runtime.Serialization;

namespace VideoOS.ConfigurationAPI.ConfigurationFaultException;

[Serializable]
public class PathNotFoundException : Exception
{
	public PathNotFoundException()
		: base(string.Empty)
	{
	}

	public PathNotFoundException(string message)
		: base(message)
	{
	}

	public PathNotFoundException(string message, Exception exception)
		: base(message, exception)
	{
	}

	protected PathNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
