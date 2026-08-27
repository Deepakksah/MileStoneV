using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Hosting;

[Serializable]
public class RegistrationException : SecureTunnelServerException
{
	public RegistrationException()
	{
	}

	public RegistrationException(string message)
		: base(message)
	{
	}

	public RegistrationException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected RegistrationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
