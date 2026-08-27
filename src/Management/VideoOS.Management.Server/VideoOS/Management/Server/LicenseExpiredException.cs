using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class LicenseExpiredException : VmoServiceException
{
	protected override int ErrorCodeDefault => 64003;

	public LicenseExpiredException(string message)
		: base(message)
	{
	}

	public LicenseExpiredException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public LicenseExpiredException()
		: base(VmoResources.VMO64003)
	{
	}

	protected LicenseExpiredException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
