using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class UnlicensedRecordingServerFailoverFeatureException : VmoException
{
	public UnlicensedRecordingServerFailoverFeatureException()
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
