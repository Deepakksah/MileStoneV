using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.DeviceDiscovery;

[Serializable]
public class SaveXmlException : Exception
{
	public SaveXmlException()
	{
	}

	public SaveXmlException(string message)
		: base(message)
	{
	}

	public SaveXmlException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected SaveXmlException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
