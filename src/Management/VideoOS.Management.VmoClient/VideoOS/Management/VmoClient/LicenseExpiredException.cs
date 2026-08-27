using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class LicenseExpiredException : VmoException
{
	public LicenseExpiredException()
	{
	}

	protected LicenseExpiredException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public LicenseExpiredException(string message)
		: base(message)
	{
	}

	public LicenseExpiredException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
