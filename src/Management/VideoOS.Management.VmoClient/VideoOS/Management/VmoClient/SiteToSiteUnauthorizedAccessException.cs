using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class SiteToSiteUnauthorizedAccessException : VmoException
{
	public SiteToSiteUnauthorizedAccessException()
	{
	}

	public SiteToSiteUnauthorizedAccessException(string message)
		: base(message)
	{
	}

	public SiteToSiteUnauthorizedAccessException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected SiteToSiteUnauthorizedAccessException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
