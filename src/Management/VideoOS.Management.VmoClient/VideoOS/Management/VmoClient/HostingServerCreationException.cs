using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class HostingServerCreationException : VmoException
{
	public HostingServerCreationException()
	{
	}

	public HostingServerCreationException(string message)
		: base(message)
	{
	}

	public HostingServerCreationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected HostingServerCreationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
