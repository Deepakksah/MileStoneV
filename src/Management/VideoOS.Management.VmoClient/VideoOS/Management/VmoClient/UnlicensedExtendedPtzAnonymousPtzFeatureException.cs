using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class UnlicensedExtendedPtzAnonymousPtzFeatureException : VmoException
{
	public UnlicensedExtendedPtzAnonymousPtzFeatureException()
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
