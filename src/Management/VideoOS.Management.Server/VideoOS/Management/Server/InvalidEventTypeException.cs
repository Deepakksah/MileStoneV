using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[Serializable]
public class InvalidEventTypeException : Exception
{
	public InvalidEventTypeException()
	{
	}

	public InvalidEventTypeException(string message)
		: base(message)
	{
	}

	public InvalidEventTypeException(string message, Exception ex)
		: base(message, ex)
	{
	}

	protected InvalidEventTypeException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
