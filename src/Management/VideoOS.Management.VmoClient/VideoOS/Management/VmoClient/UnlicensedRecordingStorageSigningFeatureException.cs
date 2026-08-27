using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class UnlicensedRecordingStorageSigningFeatureException : VmoException
{
	public UnlicensedRecordingStorageSigningFeatureException()
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
