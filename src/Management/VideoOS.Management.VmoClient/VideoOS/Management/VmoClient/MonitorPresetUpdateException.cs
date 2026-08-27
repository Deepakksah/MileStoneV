using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class MonitorPresetUpdateException : VmoException
{
	public MonitorPresetUpdateException()
	{
	}

	public MonitorPresetUpdateException(string message)
		: base(message)
	{
	}

	public MonitorPresetUpdateException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected MonitorPresetUpdateException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
