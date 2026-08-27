using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class ServerGroupNotFoundException : VmoException
{
	public ServerGroupNotFoundException()
	{
	}

	public ServerGroupNotFoundException(string message)
		: base(message)
	{
	}

	public ServerGroupNotFoundException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected ServerGroupNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
