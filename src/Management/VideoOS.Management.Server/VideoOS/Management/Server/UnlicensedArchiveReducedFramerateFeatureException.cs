using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class UnlicensedArchiveReducedFramerateFeatureException : VmoServiceException
{
	protected override int ErrorCodeDefault => 64010;

	public UnlicensedArchiveReducedFramerateFeatureException()
		: base(VmoResources.VMO64010)
	{
	}

	protected UnlicensedArchiveReducedFramerateFeatureException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public UnlicensedArchiveReducedFramerateFeatureException(string message)
		: base(message)
	{
	}

	public UnlicensedArchiveReducedFramerateFeatureException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
