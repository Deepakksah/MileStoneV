using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class MonitorCreationException : VmoException
{
	public MonitorCreationException()
	{
	}

	public MonitorCreationException(string message)
		: base(message)
	{
	}

	public MonitorCreationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected MonitorCreationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
