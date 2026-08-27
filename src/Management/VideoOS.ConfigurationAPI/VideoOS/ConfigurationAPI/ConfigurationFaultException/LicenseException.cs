using System;
using System.Runtime.Serialization;

namespace VideoOS.ConfigurationAPI.ConfigurationFaultException;

[Serializable]
public class LicenseException : Exception
{
	public LicenseException()
	{
	}

	public LicenseException(string message)
		: base(message)
	{
	}

	public LicenseException(string message, Exception exception)
		: base(message, exception)
	{
	}

	protected LicenseException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
