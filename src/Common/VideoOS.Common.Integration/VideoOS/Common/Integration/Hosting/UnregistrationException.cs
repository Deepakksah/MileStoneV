using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Hosting;

[Serializable]
public class UnregistrationException : SecureTunnelServerException
{
	public UnregistrationException()
	{
	}

	public UnregistrationException(string message)
		: base(message)
	{
	}

	public UnregistrationException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected UnregistrationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
