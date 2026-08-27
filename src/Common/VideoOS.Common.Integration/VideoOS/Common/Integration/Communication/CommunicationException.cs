using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
public class CommunicationException : Exception
{
	protected CommunicationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public CommunicationException()
	{
	}

	public CommunicationException(string message)
		: base(message)
	{
	}

	public CommunicationException(string message, Exception inner)
		: base(message, inner)
	{
	}
}
