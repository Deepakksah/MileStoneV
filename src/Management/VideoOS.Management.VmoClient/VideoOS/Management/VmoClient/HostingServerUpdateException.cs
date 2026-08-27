using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class HostingServerUpdateException : VmoException
{
	public HostingServerUpdateException()
	{
	}

	public HostingServerUpdateException(string message)
		: base(message)
	{
	}

	public HostingServerUpdateException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected HostingServerUpdateException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
