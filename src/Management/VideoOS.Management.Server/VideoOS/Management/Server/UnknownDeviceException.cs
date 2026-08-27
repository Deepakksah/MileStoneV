using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[Serializable]
public class UnknownDeviceException : Exception
{
	public UnknownDeviceException()
	{
	}

	public UnknownDeviceException(string message)
		: base(message)
	{
	}

	public UnknownDeviceException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected UnknownDeviceException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
