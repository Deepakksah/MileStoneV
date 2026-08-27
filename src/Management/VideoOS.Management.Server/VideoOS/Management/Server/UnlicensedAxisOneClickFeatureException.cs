using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class UnlicensedAxisOneClickFeatureException : VmoServiceException
{
	protected override int ErrorCodeDefault => 64012;

	public UnlicensedAxisOneClickFeatureException()
		: base(VmoResources.VMO64012)
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
