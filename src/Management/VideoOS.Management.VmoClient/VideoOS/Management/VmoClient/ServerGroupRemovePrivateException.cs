using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class ServerGroupRemovePrivateException : VmoException
{
	public ServerGroupRemovePrivateException()
	{
	}

	public ServerGroupRemovePrivateException(string message)
		: base(message)
	{
	}

	public ServerGroupRemovePrivateException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected ServerGroupRemovePrivateException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
