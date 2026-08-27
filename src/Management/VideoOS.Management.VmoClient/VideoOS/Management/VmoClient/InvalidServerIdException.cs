using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class InvalidServerIdException : VmoException
{
	public InvalidServerIdException()
	{
	}

	public InvalidServerIdException(string id)
		: base(id)
	{
	}

	public InvalidServerIdException(string id, Exception innerException)
		: base(id, innerException)
	{
	}

	protected InvalidServerIdException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
