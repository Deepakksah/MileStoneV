using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class UnlicensedRecordingStorageSigningFeatureException : VmoServiceException
{
	protected override int ErrorCodeDefault => 64020;

	public UnlicensedRecordingStorageSigningFeatureException()
		: base(VmoResources.VMO64020)
	{
	}

	protected UnlicensedRecordingStorageSigningFeatureException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public UnlicensedRecordingStorageSigningFeatureException(string message)
		: base(message)
	{
	}

	public UnlicensedRecordingStorageSigningFeatureException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
