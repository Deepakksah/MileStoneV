using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.Server.ConfigurationApi;

[Serializable]
public class ConfigurationItemInvalidException : Exception
{
	public ConfigurationItemInvalidException()
	{
	}

	public ConfigurationItemInvalidException(string message)
		: base(message)
	{
	}

	public ConfigurationItemInvalidException(string message, Exception exception)
		: base(message, exception)
	{
	}

	protected ConfigurationItemInvalidException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
