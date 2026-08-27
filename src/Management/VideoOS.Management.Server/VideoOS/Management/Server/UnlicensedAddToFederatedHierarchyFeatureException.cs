using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class UnlicensedAddToFederatedHierarchyFeatureException : VmoServiceException
{
	protected override int ErrorCodeDefault => 64035;

	public UnlicensedAddToFederatedHierarchyFeatureException()
		: base(VmoResources.VMO64035)
	{
	}

	protected UnlicensedAddToFederatedHierarchyFeatureException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public UnlicensedAddToFederatedHierarchyFeatureException(string message)
		: base(message)
	{
	}

	public UnlicensedAddToFederatedHierarchyFeatureException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
