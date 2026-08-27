using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration;

[Serializable]
public class StorageStreamAlreadyExistsException : Exception
{
	public StorageStreamAlreadyExistsException()
	{
	}

	public StorageStreamAlreadyExistsException(string message)
		: base(message)
	{
	}

	public StorageStreamAlreadyExistsException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected StorageStreamAlreadyExistsException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
