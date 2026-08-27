using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class ServerGroupUpdateException : VmoException
{
	public ServerGroupUpdateException()
	{
	}

	public ServerGroupUpdateException(string message)
		: base(message)
	{
	}

	public ServerGroupUpdateException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected ServerGroupUpdateException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
