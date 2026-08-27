using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class UnlicensedExtendedPtzLockedPresetFeatureException : VmoException
{
	public UnlicensedExtendedPtzLockedPresetFeatureException()
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
