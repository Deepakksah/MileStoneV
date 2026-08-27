using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class GeneralLicenseException : VmoServiceException
{
	protected override int ErrorCodeDefault => 64000;

	public GeneralLicenseException()
		: base(VmoResources.VMO64000)
	{
	}

	protected GeneralLicenseException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public GeneralLicenseException(string message)
		: base(message)
	{
	}

	public GeneralLicenseException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
