using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class SiteToSiteConnectionFailureException : VmoException
{
	public SiteToSiteConnectionFailureException()
	{
	}

	public SiteToSiteConnectionFailureException(string message)
		: base(message)
	{
	}

	public SiteToSiteConnectionFailureException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected SiteToSiteConnectionFailureException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
