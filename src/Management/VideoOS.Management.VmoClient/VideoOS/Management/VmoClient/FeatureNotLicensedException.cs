using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class FeatureNotLicensedException : VmoException
{
	public FeatureNotLicensedException()
	{
	}

	protected FeatureNotLicensedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public FeatureNotLicensedException(string message)
		: base(message)
	{
	}

	public FeatureNotLicensedException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
