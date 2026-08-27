using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class TimeProfileUpdateException : VmoException
{
	public TimeProfileUpdateException()
	{
	}

	public TimeProfileUpdateException(string message)
		: base(message)
	{
	}

	public TimeProfileUpdateException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected TimeProfileUpdateException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
