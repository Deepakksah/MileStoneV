using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
public class CommunicationConnectException : CommunicationException
{
	protected CommunicationConnectException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public CommunicationConnectException()
		: this("", null)
	{
	}

	public CommunicationConnectException(string message)
		: this(message, null)
	{
	}

	public CommunicationConnectException(string message, Exception inner)
		: base("Connect error: " + message, inner)
	{
	}
}
