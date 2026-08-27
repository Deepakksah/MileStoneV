using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class UnlicensedInterconnectedProductsFeatureException : VmoException
{
	public UnlicensedInterconnectedProductsFeatureException()
	{
	}

	protected UnlicensedInterconnectedProductsFeatureException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public UnlicensedInterconnectedProductsFeatureException(string message)
		: base(message)
	{
	}

	public UnlicensedInterconnectedProductsFeatureException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
