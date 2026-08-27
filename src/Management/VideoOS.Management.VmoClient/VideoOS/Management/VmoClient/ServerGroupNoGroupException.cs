using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class ServerGroupNoGroupException : VmoException
{
	public ServerGroupNoGroupException()
	{
	}

	public ServerGroupNoGroupException(string message)
		: base(message)
	{
	}

	public ServerGroupNoGroupException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected ServerGroupNoGroupException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
