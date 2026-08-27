using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
public class CommunicationTransmissionException : CommunicationException
{
	protected CommunicationTransmissionException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public CommunicationTransmissionException()
		: this("", null)
	{
	}

	public CommunicationTransmissionException(string message)
		: this(message, null)
	{
	}

	public CommunicationTransmissionException(string message, Exception inner)
		: base("Transmission error: " + message, inner)
	{
	}
}
