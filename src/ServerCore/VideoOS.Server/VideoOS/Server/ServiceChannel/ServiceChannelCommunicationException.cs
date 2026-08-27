using System;
using System.Runtime.Serialization;

namespace VideoOS.Server.ServiceChannel;

[Serializable]
public class ServiceChannelCommunicationException : ServiceChannelException
{
	public ServiceChannelCommunicationException()
	{
	}

	public ServiceChannelCommunicationException(string message)
		: base(message)
	{
	}

	public ServiceChannelCommunicationException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected ServiceChannelCommunicationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
