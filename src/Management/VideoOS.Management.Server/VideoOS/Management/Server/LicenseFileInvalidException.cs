using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class LicenseFileInvalidException : VmoServiceException
{
	protected override int ErrorCodeDefault => 64004;

	public LicenseFileInvalidException()
		: base(VmoResources.VMO64004)
	{
	}

	protected LicenseFileInvalidException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public LicenseFileInvalidException(string message)
		: base(message)
	{
	}

	public LicenseFileInvalidException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
