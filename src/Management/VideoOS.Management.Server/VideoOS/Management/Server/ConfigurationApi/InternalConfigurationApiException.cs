using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.Server.ConfigurationApi;

[Serializable]
public class InternalConfigurationApiException : Exception
{
	public InternalConfigurationApiException()
	{
	}

	public InternalConfigurationApiException(string message)
		: base(message)
	{
	}

	public InternalConfigurationApiException(string message, Exception exception)
		: base(message, exception)
	{
	}

	protected InternalConfigurationApiException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
