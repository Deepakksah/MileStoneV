using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class ClientProfileDeleteException : VmoException
{
	public ClientProfileDeleteException()
	{
	}

	public ClientProfileDeleteException(string message)
		: base(message)
	{
	}

	public ClientProfileDeleteException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected ClientProfileDeleteException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
