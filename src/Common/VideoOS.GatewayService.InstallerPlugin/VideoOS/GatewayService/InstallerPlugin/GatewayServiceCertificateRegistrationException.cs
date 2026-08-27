using System;
using System.Runtime.Serialization;

namespace VideoOS.GatewayService.InstallerPlugin;

[Serializable]
public class GatewayServiceCertificateRegistrationException : Exception
{
	public GatewayServiceCertificateRegistrationException()
	{
	}

	public GatewayServiceCertificateRegistrationException(string message)
		: base(message)
	{
	}

	public GatewayServiceCertificateRegistrationException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected GatewayServiceCertificateRegistrationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
