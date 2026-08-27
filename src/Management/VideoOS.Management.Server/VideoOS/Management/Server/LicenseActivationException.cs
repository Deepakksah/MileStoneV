using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class LicenseActivationException : VmoServiceException
{
	protected override int ErrorCodeDefault => 64000;

	public LicenseActivationException()
		: base(VmoResources.VMO64000)
	{
	}

	protected LicenseActivationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public LicenseActivationException(string message)
		: base(message)
	{
	}

	public LicenseActivationException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
