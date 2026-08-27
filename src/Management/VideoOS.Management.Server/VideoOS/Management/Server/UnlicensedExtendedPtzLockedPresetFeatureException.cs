using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class UnlicensedExtendedPtzLockedPresetFeatureException : VmoServiceException
{
	protected override int ErrorCodeDefault => 64038;

	public UnlicensedExtendedPtzLockedPresetFeatureException()
		: base(VmoResources.VMO64038)
	{
	}

	protected UnlicensedExtendedPtzLockedPresetFeatureException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public UnlicensedExtendedPtzLockedPresetFeatureException(string message)
		: base(message)
	{
	}

	public UnlicensedExtendedPtzLockedPresetFeatureException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
