using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class HostedHardwareUpdateException : VmoException
{
	public HostedHardwareUpdateException()
	{
	}

	public HostedHardwareUpdateException(string message)
		: base(message)
	{
	}

	public HostedHardwareUpdateException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected HostedHardwareUpdateException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
