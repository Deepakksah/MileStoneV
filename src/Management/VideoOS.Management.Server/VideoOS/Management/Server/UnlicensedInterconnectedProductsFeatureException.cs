using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class UnlicensedInterconnectedProductsFeatureException : VmoServiceException
{
	protected override int ErrorCodeDefault => 64014;

	public UnlicensedInterconnectedProductsFeatureException()
		: base(VmoResources.VMO64014)
	{
	}

	protected UnlicensedInterconnectedProductsFeatureException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public UnlicensedInterconnectedProductsFeatureException(string message)
		: base(message)
	{
	}

	public UnlicensedInterconnectedProductsFeatureException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
