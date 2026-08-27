using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[Serializable]
public class ConfigurationDataSubsystemException : Exception
{
	public ConfigurationDataSubsystemException()
	{
	}

	public ConfigurationDataSubsystemException(string message)
		: base(message)
	{
	}

	public ConfigurationDataSubsystemException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected ConfigurationDataSubsystemException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
