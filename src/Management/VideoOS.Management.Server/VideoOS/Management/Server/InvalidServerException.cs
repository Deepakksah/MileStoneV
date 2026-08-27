using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[Serializable]
public class InvalidServerException : Exception
{
	public InvalidServerException()
	{
	}

	public InvalidServerException(string message)
		: base(message)
	{
	}

	public InvalidServerException(string message, Exception ex)
		: base(message, ex)
	{
	}

	protected InvalidServerException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
