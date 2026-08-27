using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class HostingServerUpdateNotExistException : VmoException
{
	public HostingServerUpdateNotExistException()
	{
	}

	public HostingServerUpdateNotExistException(string message)
		: base(message)
	{
	}

	public HostingServerUpdateNotExistException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected HostingServerUpdateNotExistException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
