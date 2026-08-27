using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class ClientProfileNotExistException : VmoException
{
	public ClientProfileNotExistException()
	{
	}

	public ClientProfileNotExistException(string message)
		: base(message)
	{
	}

	public ClientProfileNotExistException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected ClientProfileNotExistException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
