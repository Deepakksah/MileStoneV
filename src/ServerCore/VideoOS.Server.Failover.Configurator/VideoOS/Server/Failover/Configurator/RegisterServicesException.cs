using System;
using System.Runtime.Serialization;

namespace VideoOS.Server.Failover.Configurator;

[Serializable]
public class RegisterServicesException : Exception
{
	public RegisterServicesException()
	{
	}

	public RegisterServicesException(string message)
		: base(message)
	{
	}

	public RegisterServicesException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected RegisterServicesException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
