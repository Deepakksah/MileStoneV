using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class UnlicensedExtendedPtzAnonymousPtzFeatureException : VmoServiceException
{
	protected override int ErrorCodeDefault => 64037;

	public UnlicensedExtendedPtzAnonymousPtzFeatureException()
		: base(VmoResources.VMO64037)
	{
	}

	protected UnlicensedExtendedPtzAnonymousPtzFeatureException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public UnlicensedExtendedPtzAnonymousPtzFeatureException(string message)
		: base(message)
	{
	}

	public UnlicensedExtendedPtzAnonymousPtzFeatureException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
