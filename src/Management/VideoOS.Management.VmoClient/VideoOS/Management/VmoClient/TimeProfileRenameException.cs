using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class TimeProfileRenameException : VmoException
{
	public TimeProfileRenameException()
	{
	}

	public TimeProfileRenameException(string message)
		: base(message)
	{
	}

	public TimeProfileRenameException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected TimeProfileRenameException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
