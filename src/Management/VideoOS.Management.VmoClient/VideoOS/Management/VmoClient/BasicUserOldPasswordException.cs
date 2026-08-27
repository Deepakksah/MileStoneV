using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class BasicUserOldPasswordException : VmoException
{
	public BasicUserOldPasswordException()
	{
	}

	public BasicUserOldPasswordException(string message)
		: base(message)
	{
	}

	public BasicUserOldPasswordException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected BasicUserOldPasswordException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
