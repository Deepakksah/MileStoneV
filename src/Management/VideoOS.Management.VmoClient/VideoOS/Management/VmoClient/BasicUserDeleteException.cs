using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class BasicUserDeleteException : VmoException
{
	public BasicUserDeleteException()
	{
	}

	public BasicUserDeleteException(string message)
		: base(message)
	{
	}

	public BasicUserDeleteException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected BasicUserDeleteException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
