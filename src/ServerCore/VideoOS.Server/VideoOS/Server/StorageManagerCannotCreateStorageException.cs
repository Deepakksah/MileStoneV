using System;
using System.Runtime.Serialization;

namespace VideoOS.Server;

[Serializable]
public class StorageManagerCannotCreateStorageException : Exception
{
	public StorageManagerCannotCreateStorageException()
	{
	}

	public StorageManagerCannotCreateStorageException(string message)
		: base(message)
	{
	}

	public StorageManagerCannotCreateStorageException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected StorageManagerCannotCreateStorageException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
