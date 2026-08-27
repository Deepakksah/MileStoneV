using System;
using System.Runtime.Serialization;

namespace VideoOS.Installer.Common.Utilities;

[Serializable]
public class ConfigurationDataException : Exception
{
	public ConfigurationDataException()
	{
	}

	public ConfigurationDataException(string message)
		: base(message)
	{
	}

	public ConfigurationDataException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected ConfigurationDataException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
