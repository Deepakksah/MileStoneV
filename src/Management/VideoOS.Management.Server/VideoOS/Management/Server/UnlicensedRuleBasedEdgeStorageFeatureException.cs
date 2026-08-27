using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class UnlicensedRuleBasedEdgeStorageFeatureException : VmoServiceException
{
	protected override int ErrorCodeDefault => 64031;

	public UnlicensedRuleBasedEdgeStorageFeatureException()
		: base(VmoResources.VMO64031)
	{
	}

	protected UnlicensedRuleBasedEdgeStorageFeatureException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public UnlicensedRuleBasedEdgeStorageFeatureException(string message)
		: base(message)
	{
	}

	public UnlicensedRuleBasedEdgeStorageFeatureException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
