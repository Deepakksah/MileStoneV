using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class UnlicensedRuleBasedBookmarkingFeatureException : VmoServiceException
{
	protected override int ErrorCodeDefault => 64022;

	public UnlicensedRuleBasedBookmarkingFeatureException()
		: base(VmoResources.VMO64022)
	{
	}

	protected UnlicensedRuleBasedBookmarkingFeatureException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public UnlicensedRuleBasedBookmarkingFeatureException(string message)
		: base(message)
	{
	}

	public UnlicensedRuleBasedBookmarkingFeatureException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
