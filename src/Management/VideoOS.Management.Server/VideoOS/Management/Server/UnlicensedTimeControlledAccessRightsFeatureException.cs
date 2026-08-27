using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class UnlicensedTimeControlledAccessRightsFeatureException : VmoServiceException
{
	protected override int ErrorCodeDefault => 64011;

	public UnlicensedTimeControlledAccessRightsFeatureException()
		: base(VmoResources.VMO64011)
	{
	}

	protected UnlicensedTimeControlledAccessRightsFeatureException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public UnlicensedTimeControlledAccessRightsFeatureException(string message)
		: base(message)
	{
	}

	public UnlicensedTimeControlledAccessRightsFeatureException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
