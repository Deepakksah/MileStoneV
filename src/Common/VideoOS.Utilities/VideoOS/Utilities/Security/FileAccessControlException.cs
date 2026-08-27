using System;

namespace VideoOS.Utilities.Security;

public class FileAccessControlException : Exception
{
	public FileAccessControlException()
	{
	}

	public FileAccessControlException(string message)
		: base(message)
	{
	}

	public FileAccessControlException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
