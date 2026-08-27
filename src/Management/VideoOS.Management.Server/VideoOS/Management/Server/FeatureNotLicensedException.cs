using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class FeatureNotLicensedException : VmoServiceException
{
	protected override int ErrorCodeDefault => 64001;

	public FeatureNotLicensedException(string message)
		: base(message)
	{
	}

	public FeatureNotLicensedException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public FeatureNotLicensedException()
		: base(VmoResources.VMO64001)
	{
	}

	public FeatureNotLicensedException(int id)
		: base(VmoResources.VMO64001, id)
	{
	}

	public FeatureNotLicensedException(string message, int id)
		: base(message, id)
	{
	}

	protected FeatureNotLicensedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
