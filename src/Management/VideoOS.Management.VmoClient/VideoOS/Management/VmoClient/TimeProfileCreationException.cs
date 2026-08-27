using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class TimeProfileCreationException : VmoException
{
	public TimeProfileCreationException()
	{
	}

	public TimeProfileCreationException(string message)
		: base(message)
	{
	}

	public TimeProfileCreationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected TimeProfileCreationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
