using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
public class CommunicationDisconnectException : CommunicationException
{
	protected CommunicationDisconnectException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public CommunicationDisconnectException()
		: this("", null)
	{
	}

	public CommunicationDisconnectException(string message)
		: this(message, null)
	{
	}

	public CommunicationDisconnectException(string message, Exception inner)
		: base("Connect error: " + message, inner)
	{
	}
}
