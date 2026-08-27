using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class BasicUserSecureConnectionException : VmoException
{
	public BasicUserSecureConnectionException()
	{
	}

	public BasicUserSecureConnectionException(string message)
		: base(message)
	{
	}

	public BasicUserSecureConnectionException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected BasicUserSecureConnectionException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
