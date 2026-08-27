using System;

namespace VideoOS.Utilities.Database;

public class ConnectionStorageReadException : ConnectionStorageException
{
	public ConnectionStorageReadException(string message)
		: base(message)
	{
	}

	public ConnectionStorageReadException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
