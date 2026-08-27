using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class DevicePtzPresetNotFoundException : VmoException
{
	public DevicePtzPresetNotFoundException()
	{
	}

	public DevicePtzPresetNotFoundException(string message)
		: base(message)
	{
	}

	public DevicePtzPresetNotFoundException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected DevicePtzPresetNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
