using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[Serializable]
public sealed class MarkedDataDevicesFailedException : Exception
{
	public MarkedDataDevicesFailedException()
	{
	}

	public MarkedDataDevicesFailedException(string message)
		: base(message)
	{
	}

	public MarkedDataDevicesFailedException(string message, Exception inner)
		: base(message, inner)
	{
	}

	private MarkedDataDevicesFailedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
