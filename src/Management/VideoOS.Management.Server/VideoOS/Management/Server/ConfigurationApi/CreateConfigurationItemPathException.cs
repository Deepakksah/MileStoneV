using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.Server.ConfigurationApi;

[Serializable]
public class CreateConfigurationItemPathException : Exception
{
	public CreateConfigurationItemPathException()
	{
	}

	public CreateConfigurationItemPathException(string message)
		: base(message)
	{
	}

	public CreateConfigurationItemPathException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected CreateConfigurationItemPathException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
