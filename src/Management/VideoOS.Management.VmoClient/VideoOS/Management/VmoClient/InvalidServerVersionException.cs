using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class InvalidServerVersionException : VmoException
{
	public InvalidServerVersionException()
	{
	}

	public InvalidServerVersionException(string message)
		: base(message)
	{
	}

	public InvalidServerVersionException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected InvalidServerVersionException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
