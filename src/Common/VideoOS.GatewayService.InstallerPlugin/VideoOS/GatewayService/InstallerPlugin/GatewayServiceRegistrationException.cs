using System;
using System.Runtime.Serialization;

namespace VideoOS.GatewayService.InstallerPlugin;

[Serializable]
public class GatewayServiceRegistrationException : Exception
{
	public GatewayServiceRegistrationException()
	{
	}

	public GatewayServiceRegistrationException(string message)
		: base(message)
	{
	}

	public GatewayServiceRegistrationException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected GatewayServiceRegistrationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
