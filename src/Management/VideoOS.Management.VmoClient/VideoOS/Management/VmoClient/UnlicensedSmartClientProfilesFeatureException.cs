using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class UnlicensedSmartClientProfilesFeatureException : VmoException
{
	public UnlicensedSmartClientProfilesFeatureException()
	{
	}

	protected UnlicensedSmartClientProfilesFeatureException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public UnlicensedSmartClientProfilesFeatureException(string message)
		: base(message)
	{
	}

	public UnlicensedSmartClientProfilesFeatureException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
