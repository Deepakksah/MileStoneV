using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
public class CommunicationInitializationException : CommunicationException
{
	protected CommunicationInitializationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public CommunicationInitializationException()
		: this("", null)
	{
	}

	public CommunicationInitializationException(string message)
		: this(message, null)
	{
	}

	public CommunicationInitializationException(string message, Exception inner)
		: base("Initialization error: " + message, inner)
	{
	}
}
