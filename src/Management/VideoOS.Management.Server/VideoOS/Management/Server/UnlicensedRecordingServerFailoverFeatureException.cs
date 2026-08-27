using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class UnlicensedRecordingServerFailoverFeatureException : VmoServiceException
{
	protected override int ErrorCodeDefault => 64018;

	public UnlicensedRecordingServerFailoverFeatureException()
		: base(VmoResources.VMO64018)
	{
	}

	protected UnlicensedRecordingServerFailoverFeatureException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public UnlicensedRecordingServerFailoverFeatureException(string message)
		: base(message)
	{
	}

	public UnlicensedRecordingServerFailoverFeatureException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
