using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class UnlicensedRemoteConnectedServicesFeatureException : VmoServiceException
{
	protected override int ErrorCodeDefault => 64021;

	public UnlicensedRemoteConnectedServicesFeatureException()
		: base(VmoResources.VMO64021)
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
