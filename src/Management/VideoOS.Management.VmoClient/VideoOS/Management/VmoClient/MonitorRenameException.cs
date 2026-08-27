using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class MonitorRenameException : VmoException
{
	public MonitorRenameException()
	{
	}

	public MonitorRenameException(string message)
		: base(message)
	{
	}

	public MonitorRenameException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected MonitorRenameException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
