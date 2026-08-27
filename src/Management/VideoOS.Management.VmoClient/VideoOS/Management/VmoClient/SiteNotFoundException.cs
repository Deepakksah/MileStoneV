using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class SiteNotFoundException : VmoException
{
	public SiteNotFoundException()
	{
	}

	public SiteNotFoundException(string message)
		: base(message)
	{
	}

	public SiteNotFoundException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected SiteNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
