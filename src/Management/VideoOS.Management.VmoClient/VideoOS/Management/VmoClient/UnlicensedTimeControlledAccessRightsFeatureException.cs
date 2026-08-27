using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class UnlicensedTimeControlledAccessRightsFeatureException : VmoException
{
	public UnlicensedTimeControlledAccessRightsFeatureException()
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
