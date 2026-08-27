using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class UnlicensedLiveMultistreamingFeatureException : VmoServiceException
{
	protected override int ErrorCodeDefault => 64015;

	public UnlicensedLiveMultistreamingFeatureException()
		: base(VmoResources.VMO64015)
	{
	}

	protected UnlicensedLiveMultistreamingFeatureException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public UnlicensedLiveMultistreamingFeatureException(string message)
		: base(message)
	{
	}

	public UnlicensedLiveMultistreamingFeatureException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
