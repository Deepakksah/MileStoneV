using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class MonitorUpdateException : VmoException
{
	public MonitorUpdateException()
	{
	}

	public MonitorUpdateException(string message)
		: base(message)
	{
	}

	public MonitorUpdateException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected MonitorUpdateException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
