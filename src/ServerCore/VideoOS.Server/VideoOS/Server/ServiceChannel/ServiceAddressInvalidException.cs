using System;
using System.Runtime.Serialization;

namespace VideoOS.Server.ServiceChannel;

[Serializable]
public class ServiceAddressInvalidException : ServiceChannelException
{
	public ServiceAddressInvalidException()
	{
	}

	public ServiceAddressInvalidException(string message)
		: base(message)
	{
	}

	public ServiceAddressInvalidException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected ServiceAddressInvalidException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
