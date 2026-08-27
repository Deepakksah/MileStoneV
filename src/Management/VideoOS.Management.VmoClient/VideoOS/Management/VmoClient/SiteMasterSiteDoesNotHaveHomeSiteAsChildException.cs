using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class SiteMasterSiteDoesNotHaveHomeSiteAsChildException : VmoException
{
	public SiteMasterSiteDoesNotHaveHomeSiteAsChildException()
	{
	}

	public SiteMasterSiteDoesNotHaveHomeSiteAsChildException(string message)
		: base(message)
	{
	}

	public SiteMasterSiteDoesNotHaveHomeSiteAsChildException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected SiteMasterSiteDoesNotHaveHomeSiteAsChildException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
