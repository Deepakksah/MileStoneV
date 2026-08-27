using System;

namespace VideoOS.Utilities.Database.KeyManagement;

public sealed class KeyStorageException : Exception
{
	public KeyStorageErrorCode ErrorType { get; }

	public KeyStorageException(KeyStorageErrorCode errorType, string message)
		: base(message)
	{
		ErrorType = errorType;
	}

	public KeyStorageException(KeyStorageErrorCode errorType, string message, Exception innerException)
		: base(message, innerException)
	{
		ErrorType = errorType;
	}
}
