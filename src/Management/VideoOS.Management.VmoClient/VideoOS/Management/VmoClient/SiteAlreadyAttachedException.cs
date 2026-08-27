using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class SiteAlreadyAttachedException : VmoException
{
	public SiteAlreadyAttachedException()
	{
	}

	public SiteAlreadyAttachedException(string message)
		: base(message)
	{
	}

	public SiteAlreadyAttachedException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected SiteAlreadyAttachedException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
