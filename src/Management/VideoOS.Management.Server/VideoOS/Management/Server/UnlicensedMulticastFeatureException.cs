using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class UnlicensedMulticastFeatureException : VmoServiceException
{
	protected override int ErrorCodeDefault => 64016;

	public UnlicensedMulticastFeatureException()
		: base(VmoResources.VMO64016)
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
