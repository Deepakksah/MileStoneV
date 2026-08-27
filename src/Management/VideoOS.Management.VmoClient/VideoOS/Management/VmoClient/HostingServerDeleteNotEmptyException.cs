using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class HostingServerDeleteNotEmptyException : VmoException
{
	public HostingServerDeleteNotEmptyException()
	{
	}

	public HostingServerDeleteNotEmptyException(string message)
		: base(message)
	{
	}

	public HostingServerDeleteNotEmptyException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected HostingServerDeleteNotEmptyException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
