using System;

namespace VideoOS.Utilities.Database;

public sealed class ConnectionStorageSetException : ConnectionStorageException
{
	public ConnectionStorageSetException(string message)
		: base(message)
	{
	}

	public ConnectionStorageSetException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
