using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class UnlicensedRuleBasedPtzPriorityFeatureException : VmoServiceException
{
	protected override int ErrorCodeDefault => 64032;

	public UnlicensedRuleBasedPtzPriorityFeatureException()
		: base(VmoResources.VMO64032)
	{
	}

	protected UnlicensedRuleBasedPtzPriorityFeatureException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public UnlicensedRuleBasedPtzPriorityFeatureException(string message)
		: base(message)
	{
	}

	public UnlicensedRuleBasedPtzPriorityFeatureException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
