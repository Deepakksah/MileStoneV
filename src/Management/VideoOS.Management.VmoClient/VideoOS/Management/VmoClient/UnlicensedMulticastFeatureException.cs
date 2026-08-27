using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class UnlicensedMulticastFeatureException : VmoException
{
	public UnlicensedMulticastFeatureException()
	{
	}

	protected UnlicensedMulticastFeatureException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public UnlicensedMulticastFeatureException(string message)
		: base(message)
	{
	}

	public UnlicensedMulticastFeatureException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
