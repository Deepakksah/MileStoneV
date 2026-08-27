using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class UnlicensedRemoteConnectedServicesFeatureException : VmoException
{
	public UnlicensedRemoteConnectedServicesFeatureException()
	{
	}

	protected UnlicensedRemoteConnectedServicesFeatureException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public UnlicensedRemoteConnectedServicesFeatureException(string message)
		: base(message)
	{
	}

	public UnlicensedRemoteConnectedServicesFeatureException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
