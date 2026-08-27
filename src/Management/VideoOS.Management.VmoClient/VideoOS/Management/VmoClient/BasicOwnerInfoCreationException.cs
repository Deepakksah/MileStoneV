using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class BasicOwnerInfoCreationException : VmoException
{
	public BasicOwnerInfoCreationException()
	{
	}

	public BasicOwnerInfoCreationException(string message)
		: base(message)
	{
	}

	public BasicOwnerInfoCreationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected BasicOwnerInfoCreationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
