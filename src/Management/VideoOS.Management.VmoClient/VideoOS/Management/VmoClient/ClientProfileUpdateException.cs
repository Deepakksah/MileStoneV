using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class ClientProfileUpdateException : VmoException
{
	public ClientProfileUpdateException()
	{
	}

	public ClientProfileUpdateException(string message)
		: base(message)
	{
	}

	public ClientProfileUpdateException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected ClientProfileUpdateException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
