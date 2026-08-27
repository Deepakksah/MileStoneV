using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class FeatureNotInstalledException : VmoException
{
	public FeatureNotInstalledException()
	{
	}

	public FeatureNotInstalledException(string message)
		: base(message)
	{
	}

	public FeatureNotInstalledException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected FeatureNotInstalledException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
