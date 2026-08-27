using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class UnlicensedRecordingStorageEncryptionFeatureException : VmoException
{
	public UnlicensedRecordingStorageEncryptionFeatureException()
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
