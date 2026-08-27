using System;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class InvalidDomainException : SecuritySubsystemException
{
	protected override int ErrorCodeDefault => -1;

	public InvalidDomainException()
	{
	}

	public InvalidDomainException(string domain)
		: base(domain, null)
	{
	}

	protected InvalidDomainException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public InvalidDomainException(string domain, Exception innerException)
		: base(string.Format(CultureInfo.CurrentCulture, VmoResources.VMO62018, domain), innerException)
	{
	}
}
