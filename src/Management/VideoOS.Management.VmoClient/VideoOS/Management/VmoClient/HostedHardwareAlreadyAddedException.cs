using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class HostedHardwareAlreadyAddedException : VmoException
{
	public HostedHardwareAlreadyAddedException()
	{
	}

	public HostedHardwareAlreadyAddedException(string message)
		: base(message)
	{
	}

	public HostedHardwareAlreadyAddedException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected HostedHardwareAlreadyAddedException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
