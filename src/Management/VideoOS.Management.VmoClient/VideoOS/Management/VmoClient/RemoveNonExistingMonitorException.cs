using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class RemoveNonExistingMonitorException : VmoException
{
	public RemoveNonExistingMonitorException()
	{
	}

	public RemoveNonExistingMonitorException(string message)
		: base(message)
	{
	}

	public RemoveNonExistingMonitorException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected RemoveNonExistingMonitorException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
