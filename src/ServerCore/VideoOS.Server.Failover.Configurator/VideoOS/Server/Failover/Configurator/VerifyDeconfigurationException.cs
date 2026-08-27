using System;
using System.Runtime.Serialization;

namespace VideoOS.Server.Failover.Configurator;

[Serializable]
public class VerifyDeconfigurationException : Exception
{
	public VerifyDeconfigurationException()
	{
	}

	public VerifyDeconfigurationException(string message)
		: base(message)
	{
	}

	public VerifyDeconfigurationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected VerifyDeconfigurationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
