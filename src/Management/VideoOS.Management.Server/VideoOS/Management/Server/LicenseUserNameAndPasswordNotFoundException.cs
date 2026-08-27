using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class LicenseUserNameAndPasswordNotFoundException : VmoServiceException
{
	protected override int ErrorCodeDefault => 64006;

	public LicenseUserNameAndPasswordNotFoundException()
		: base(VmoResources.VMO64006)
	{
	}

	protected LicenseUserNameAndPasswordNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public LicenseUserNameAndPasswordNotFoundException(string message)
		: base(message)
	{
	}

	public LicenseUserNameAndPasswordNotFoundException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
