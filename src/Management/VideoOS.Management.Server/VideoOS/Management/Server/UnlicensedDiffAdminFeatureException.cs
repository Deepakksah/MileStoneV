using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class UnlicensedDiffAdminFeatureException : VmoServiceException
{
	protected override int ErrorCodeDefault => 64029;

	public UnlicensedDiffAdminFeatureException()
		: base(VmoResources.VMO64029)
	{
	}

	protected UnlicensedDiffAdminFeatureException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public UnlicensedDiffAdminFeatureException(string message)
		: base(message)
	{
	}

	public UnlicensedDiffAdminFeatureException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
