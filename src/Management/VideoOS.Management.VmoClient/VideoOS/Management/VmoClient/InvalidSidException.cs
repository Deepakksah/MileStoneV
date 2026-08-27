using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class InvalidSidException : VmoException
{
	public InvalidSidException()
	{
	}

	public InvalidSidException(string message)
		: base(message)
	{
	}

	public InvalidSidException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected InvalidSidException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
