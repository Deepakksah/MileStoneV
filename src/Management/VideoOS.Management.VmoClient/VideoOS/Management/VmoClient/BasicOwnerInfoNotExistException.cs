using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class BasicOwnerInfoNotExistException : VmoException
{
	public BasicOwnerInfoNotExistException()
	{
	}

	public BasicOwnerInfoNotExistException(string message)
		: base(message)
	{
	}

	public BasicOwnerInfoNotExistException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected BasicOwnerInfoNotExistException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
