using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class UnlicensedAxisOneClickFeatureException : VmoException
{
	public UnlicensedAxisOneClickFeatureException()
	{
	}

	protected UnlicensedAxisOneClickFeatureException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public UnlicensedAxisOneClickFeatureException(string message)
		: base(message)
	{
	}

	public UnlicensedAxisOneClickFeatureException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
