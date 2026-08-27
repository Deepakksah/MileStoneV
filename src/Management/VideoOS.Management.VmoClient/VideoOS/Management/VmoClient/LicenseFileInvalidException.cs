using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient;

[Serializable]
public class LicenseFileInvalidException : VmoException
{
	public LicenseFileInvalidException()
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
