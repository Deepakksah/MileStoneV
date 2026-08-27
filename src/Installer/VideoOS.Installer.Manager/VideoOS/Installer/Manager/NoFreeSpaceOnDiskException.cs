using System;
using System.Runtime.Serialization;

namespace VideoOS.Installer.Manager;

[Serializable]
public class NoFreeSpaceOnDiskException : Exception
{
	public NoFreeSpaceOnDiskException()
	{
	}

	public NoFreeSpaceOnDiskException(string message)
		: base(message)
	{
	}

	public NoFreeSpaceOnDiskException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected NoFreeSpaceOnDiskException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
