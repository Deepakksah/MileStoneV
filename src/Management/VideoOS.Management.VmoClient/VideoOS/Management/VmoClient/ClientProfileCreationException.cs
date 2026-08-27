using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class ClientProfileCreationException : VmoException
{
	public ClientProfileCreationException()
	{
	}

	public ClientProfileCreationException(string message)
		: base(message)
	{
	}

	public ClientProfileCreationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected ClientProfileCreationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
