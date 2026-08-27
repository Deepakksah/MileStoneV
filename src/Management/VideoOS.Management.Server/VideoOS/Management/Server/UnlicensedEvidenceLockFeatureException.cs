using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class UnlicensedEvidenceLockFeatureException : VmoServiceException
{
	protected override int ErrorCodeDefault => 64026;

	public UnlicensedEvidenceLockFeatureException()
		: base(VmoResources.VMO64026)
	{
	}

	protected UnlicensedEvidenceLockFeatureException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public UnlicensedEvidenceLockFeatureException(string message)
		: base(message)
	{
	}

	public UnlicensedEvidenceLockFeatureException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
