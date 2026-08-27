using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class SiteRemoteSiteHasSiteAsChildException : VmoException
{
	public SiteRemoteSiteHasSiteAsChildException()
	{
	}

	public SiteRemoteSiteHasSiteAsChildException(string message)
		: base(message)
	{
	}

	public SiteRemoteSiteHasSiteAsChildException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected SiteRemoteSiteHasSiteAsChildException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
