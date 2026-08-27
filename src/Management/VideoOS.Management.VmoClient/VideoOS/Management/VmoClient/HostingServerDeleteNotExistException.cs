using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class HostingServerDeleteNotExistException : VmoException
{
	public HostingServerDeleteNotExistException()
	{
	}

	public HostingServerDeleteNotExistException(string message)
		: base(message)
	{
	}

	public HostingServerDeleteNotExistException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected HostingServerDeleteNotExistException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
