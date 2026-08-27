using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class UnlicensedRecordingStorageEncryptionFeatureException : VmoServiceException
{
	protected override int ErrorCodeDefault => 64019;

	public UnlicensedRecordingStorageEncryptionFeatureException()
		: base(VmoResources.VMO64019)
	{
	}

	protected UnlicensedRecordingStorageEncryptionFeatureException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public UnlicensedRecordingStorageEncryptionFeatureException(string message)
		: base(message)
	{
	}

	public UnlicensedRecordingStorageEncryptionFeatureException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
