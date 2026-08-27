using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
public class CommunicationNotConnectedException : CommunicationException
{
	protected CommunicationNotConnectedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public CommunicationNotConnectedException()
		: this("", null)
	{
	}

	public CommunicationNotConnectedException(string message)
		: this(message, null)
	{
	}

	public CommunicationNotConnectedException(string message, Exception inner)
		: base("Not connected: " + message, inner)
	{
	}
}
