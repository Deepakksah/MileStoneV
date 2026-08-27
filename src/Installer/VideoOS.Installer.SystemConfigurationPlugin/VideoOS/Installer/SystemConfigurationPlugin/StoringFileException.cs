using System;
using System.Runtime.Serialization;

namespace VideoOS.Installer.SystemConfigurationPlugin;

[Serializable]
public class StoringFileException : Exception
{
	public StoringFileException()
	{
	}

	public StoringFileException(string message)
		: base(message)
	{
	}

	public StoringFileException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected StoringFileException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
