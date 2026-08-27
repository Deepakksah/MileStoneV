using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class HostedHardwareDeleteNotExistException : VmoException
{
	public HostedHardwareDeleteNotExistException()
	{
	}

	public HostedHardwareDeleteNotExistException(string message)
		: base(message)
	{
	}

	public HostedHardwareDeleteNotExistException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected HostedHardwareDeleteNotExistException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
