using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class UnlicensedArchiveReducedFramerateFeatureException : VmoException
{
	public UnlicensedArchiveReducedFramerateFeatureException()
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
