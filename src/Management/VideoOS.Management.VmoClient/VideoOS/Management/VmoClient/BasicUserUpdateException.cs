using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class BasicUserUpdateException : VmoException
{
	public BasicUserUpdateException()
	{
	}

	public BasicUserUpdateException(string message)
		: base(message)
	{
	}

	public BasicUserUpdateException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected BasicUserUpdateException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
