using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.DeviceDiscovery;

[Serializable]
public class LoadXmlException : Exception
{
	public LoadXmlException()
	{
	}

	public LoadXmlException(string message)
		: base(message)
	{
	}

	public LoadXmlException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected LoadXmlException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
