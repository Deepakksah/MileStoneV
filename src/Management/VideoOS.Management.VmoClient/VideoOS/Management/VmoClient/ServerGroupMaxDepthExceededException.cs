using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class ServerGroupMaxDepthExceededException : VmoException
{
	public ServerGroupMaxDepthExceededException()
	{
	}

	public ServerGroupMaxDepthExceededException(string message)
		: base(message)
	{
	}

	public ServerGroupMaxDepthExceededException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected ServerGroupMaxDepthExceededException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
