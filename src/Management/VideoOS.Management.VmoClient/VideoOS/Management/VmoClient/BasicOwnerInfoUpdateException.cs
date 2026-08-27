using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class BasicOwnerInfoUpdateException : VmoException
{
	public BasicOwnerInfoUpdateException()
	{
	}

	public BasicOwnerInfoUpdateException(string message)
		: base(message)
	{
	}

	public BasicOwnerInfoUpdateException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected BasicOwnerInfoUpdateException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
