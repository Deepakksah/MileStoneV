using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class UnlicensedSmartWallFeatureException : VmoServiceException
{
	protected override int ErrorCodeDefault => 64024;

	public UnlicensedSmartWallFeatureException()
		: base(VmoResources.VMO64024)
	{
	}

	protected UnlicensedSmartWallFeatureException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public UnlicensedSmartWallFeatureException(string message)
		: base(message)
	{
	}

	public UnlicensedSmartWallFeatureException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
