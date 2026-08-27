using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class VmoUnauthorizedAccessException : VmoException
{
	public VmoUnauthorizedAccessException()
	{
	}

	public VmoUnauthorizedAccessException(string message)
		: base(message)
	{
	}

	public VmoUnauthorizedAccessException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected VmoUnauthorizedAccessException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
