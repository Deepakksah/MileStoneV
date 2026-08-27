using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class BasicUserNotExistException : VmoException
{
	public BasicUserNotExistException()
	{
	}

	public BasicUserNotExistException(string message)
		: base(message)
	{
	}

	public BasicUserNotExistException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected BasicUserNotExistException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
