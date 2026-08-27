using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class UnlicensedSmartWallFeatureException : VmoException
{
	public UnlicensedSmartWallFeatureException()
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
