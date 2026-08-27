using System;
using System.Runtime.Serialization;

namespace VideoOS.Installer.Common.Utilities;

[Serializable]
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

	protected FileAccessControlException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
