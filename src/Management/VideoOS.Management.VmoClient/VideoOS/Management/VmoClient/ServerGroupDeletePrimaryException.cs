using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class ServerGroupDeletePrimaryException : VmoException
{
	public ServerGroupDeletePrimaryException()
	{
	}

	public ServerGroupDeletePrimaryException(string message)
		: base(message)
	{
	}

	public ServerGroupDeletePrimaryException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected ServerGroupDeletePrimaryException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
