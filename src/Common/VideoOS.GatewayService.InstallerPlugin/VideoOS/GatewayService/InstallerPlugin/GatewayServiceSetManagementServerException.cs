using System;
using System.Runtime.Serialization;

namespace VideoOS.GatewayService.InstallerPlugin;

[Serializable]
public class GatewayServiceSetManagementServerException : Exception
{
	public GatewayServiceSetManagementServerException()
	{
	}

	public GatewayServiceSetManagementServerException(string message)
		: base(message)
	{
	}

	public GatewayServiceSetManagementServerException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected GatewayServiceSetManagementServerException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
