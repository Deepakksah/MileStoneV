using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class UnlicensedSecurityPtzPriorityFeatureException : VmoServiceException
{
	protected override int ErrorCodeDefault => 64033;

	public UnlicensedSecurityPtzPriorityFeatureException()
		: base(VmoResources.VMO64033)
	{
	}

	protected UnlicensedSecurityPtzPriorityFeatureException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public UnlicensedSecurityPtzPriorityFeatureException(string message)
		: base(message)
	{
	}

	public UnlicensedSecurityPtzPriorityFeatureException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
