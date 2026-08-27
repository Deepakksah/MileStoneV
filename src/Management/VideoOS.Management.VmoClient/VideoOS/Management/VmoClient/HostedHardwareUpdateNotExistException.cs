using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class HostedHardwareUpdateNotExistException : VmoException
{
	public HostedHardwareUpdateNotExistException()
	{
	}

	public HostedHardwareUpdateNotExistException(string message)
		: base(message)
	{
	}

	public HostedHardwareUpdateNotExistException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected HostedHardwareUpdateNotExistException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
