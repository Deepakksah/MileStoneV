using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration;

[Serializable]
public class StorageStreamNotDisposedException : Exception
{
	public StorageStreamNotDisposedException()
	{
	}

	public StorageStreamNotDisposedException(string message)
		: base(message)
	{
	}

	public StorageStreamNotDisposedException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected StorageStreamNotDisposedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
