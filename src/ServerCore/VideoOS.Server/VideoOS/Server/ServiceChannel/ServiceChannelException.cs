using System;
using System.Runtime.Serialization;

namespace VideoOS.Server.ServiceChannel;

[Serializable]
public class ServiceChannelException : Exception
{
	public ServiceChannelException()
	{
	}

	public ServiceChannelException(string message)
		: base(message)
	{
	}

	public ServiceChannelException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected ServiceChannelException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
