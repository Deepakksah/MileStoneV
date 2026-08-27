using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class HostedHardwareCreationException : VmoException
{
	public HostedHardwareCreationException()
	{
	}

	public HostedHardwareCreationException(string message)
		: base(message)
	{
	}

	public HostedHardwareCreationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected HostedHardwareCreationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
