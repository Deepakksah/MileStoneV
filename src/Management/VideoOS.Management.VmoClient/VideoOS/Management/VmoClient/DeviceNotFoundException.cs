using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class DeviceNotFoundException : VmoException
{
	public DeviceNotFoundException()
	{
	}

	public DeviceNotFoundException(string message)
		: base(message)
	{
	}

	public DeviceNotFoundException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected DeviceNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
