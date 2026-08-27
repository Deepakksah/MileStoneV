using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class BasicUserCreationException : VmoException
{
	public BasicUserCreationException()
	{
	}

	public BasicUserCreationException(string message)
		: base(message)
	{
	}

	public BasicUserCreationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected BasicUserCreationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
