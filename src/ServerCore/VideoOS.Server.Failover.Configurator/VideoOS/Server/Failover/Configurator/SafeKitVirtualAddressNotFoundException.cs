using System;
using System.Runtime.Serialization;

namespace VideoOS.Server.Failover.Configurator;

[Serializable]
public class SafeKitVirtualAddressNotFoundException : Exception
{
	public SafeKitVirtualAddressNotFoundException()
	{
	}

	public SafeKitVirtualAddressNotFoundException(string message)
		: base(message)
	{
	}

	public SafeKitVirtualAddressNotFoundException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected SafeKitVirtualAddressNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
