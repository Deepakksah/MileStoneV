using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class SiteMasterAlreadyAttachedException : VmoException
{
	public SiteMasterAlreadyAttachedException()
	{
	}

	public SiteMasterAlreadyAttachedException(string message)
		: base(message)
	{
	}

	public SiteMasterAlreadyAttachedException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected SiteMasterAlreadyAttachedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
