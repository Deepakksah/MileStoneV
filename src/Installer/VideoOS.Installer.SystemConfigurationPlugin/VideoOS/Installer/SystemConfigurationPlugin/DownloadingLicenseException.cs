using System;
using System.Runtime.Serialization;

namespace VideoOS.Installer.SystemConfigurationPlugin;

[Serializable]
public class DownloadingLicenseException : Exception
{
	public DownloadingLicenseException()
	{
	}

	public DownloadingLicenseException(string message)
		: base(message)
	{
	}

	public DownloadingLicenseException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected DownloadingLicenseException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
