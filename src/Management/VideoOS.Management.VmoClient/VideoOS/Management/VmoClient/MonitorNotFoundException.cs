using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class MonitorNotFoundException : VmoException
{
	public MonitorNotFoundException()
	{
	}

	public MonitorNotFoundException(string message)
		: base(message)
	{
	}

	public MonitorNotFoundException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected MonitorNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
