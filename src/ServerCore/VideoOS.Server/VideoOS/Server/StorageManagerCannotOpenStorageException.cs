using System;
using System.Runtime.Serialization;

namespace VideoOS.Server;

[Serializable]
public class StorageManagerCannotOpenStorageException : Exception
{
	public StorageManagerCannotOpenStorageException()
	{
	}

	public StorageManagerCannotOpenStorageException(string message)
		: base(message)
	{
	}

	public StorageManagerCannotOpenStorageException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected StorageManagerCannotOpenStorageException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
