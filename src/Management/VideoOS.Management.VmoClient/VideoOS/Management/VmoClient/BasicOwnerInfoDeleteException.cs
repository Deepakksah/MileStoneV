using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class BasicOwnerInfoDeleteException : VmoException
{
	public BasicOwnerInfoDeleteException()
	{
	}

	public BasicOwnerInfoDeleteException(string message)
		: base(message)
	{
	}

	public BasicOwnerInfoDeleteException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected BasicOwnerInfoDeleteException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
