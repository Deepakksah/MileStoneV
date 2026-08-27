using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Hosting;

[Serializable]
public class SecureTunnelServerException : Exception
{
	public SecureTunnelServerException()
	{
	}

	public SecureTunnelServerException(string message)
		: base(message)
	{
	}

	public SecureTunnelServerException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected SecureTunnelServerException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
