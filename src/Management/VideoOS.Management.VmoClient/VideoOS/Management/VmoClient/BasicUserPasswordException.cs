using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class BasicUserPasswordException : VmoException
{
	public BasicUserPasswordException()
	{
	}

	public BasicUserPasswordException(string message)
		: base(message)
	{
	}

	public BasicUserPasswordException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected BasicUserPasswordException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
