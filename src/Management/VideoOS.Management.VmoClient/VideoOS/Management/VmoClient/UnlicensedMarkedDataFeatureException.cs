using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class UnlicensedMarkedDataFeatureException : VmoException
{
	public UnlicensedMarkedDataFeatureException()
	{
	}

	protected UnlicensedMarkedDataFeatureException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public UnlicensedMarkedDataFeatureException(string message)
		: base(message)
	{
	}

	public UnlicensedMarkedDataFeatureException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
