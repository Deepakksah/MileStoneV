using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class UnlicensedLiveMultistreamingFeatureException : VmoException
{
	public UnlicensedLiveMultistreamingFeatureException()
	{
	}

	protected UnlicensedLiveMultistreamingFeatureException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public UnlicensedLiveMultistreamingFeatureException(string message)
		: base(message)
	{
	}

	public UnlicensedLiveMultistreamingFeatureException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
