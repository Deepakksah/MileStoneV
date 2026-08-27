using System;

namespace VideoOS.Utilities.Database;

public class ConnectionStorageException : Exception
{
	public ConnectionStorageException()
	{
	}

	public ConnectionStorageException(string message)
		: base(message)
	{
	}

	public ConnectionStorageException(string message, Exception inner)
		: base(message, inner)
	{
	}
}
