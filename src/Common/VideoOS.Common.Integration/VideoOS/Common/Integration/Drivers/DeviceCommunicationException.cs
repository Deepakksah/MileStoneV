using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Drivers;

[Serializable]
public class DeviceCommunicationException : Exception
{
	public DeviceCommunicationException()
	{
	}

	public DeviceCommunicationException(string message)
		: base(message)
	{
	}

	public DeviceCommunicationException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected DeviceCommunicationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
