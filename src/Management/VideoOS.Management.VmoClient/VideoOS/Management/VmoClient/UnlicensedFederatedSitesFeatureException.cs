using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class UnlicensedFederatedSitesFeatureException : VmoException
{
	public UnlicensedFederatedSitesFeatureException()
	{
	}

	protected UnlicensedFederatedSitesFeatureException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public UnlicensedFederatedSitesFeatureException(string message)
		: base(message)
	{
	}

	public UnlicensedFederatedSitesFeatureException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
