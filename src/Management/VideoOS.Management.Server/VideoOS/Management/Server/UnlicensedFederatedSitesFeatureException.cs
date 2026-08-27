using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class UnlicensedFederatedSitesFeatureException : VmoServiceException
{
	protected override int ErrorCodeDefault => 64013;

	public UnlicensedFederatedSitesFeatureException()
		: base(VmoResources.VMO64013)
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
