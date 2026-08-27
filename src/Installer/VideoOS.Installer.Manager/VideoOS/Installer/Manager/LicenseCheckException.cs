using System;
using VideoOS.Installer.Manager.Properties;

namespace VideoOS.Installer.Manager;

public class LicenseCheckException : Exception
{
	public string LicenseType { get; set; }

	public string LicenseMessage { get; set; }

	public string LogMessage { get; set; }

	public LicenseCheckException()
	{
		LicenseType = Resources.LicenseCheck_NotFoundCaption;
		LicenseMessage = Resources.LicenseCheck_NotFound;
	}

	public LicenseCheckException(string licenseMessage, string licenseType)
	{
		LicenseType = licenseType;
		LicenseMessage = licenseMessage;
	}

	public LicenseCheckException(string licenseMessage, string licenseType, string logMessage)
	{
		LicenseType = licenseType;
		LicenseMessage = licenseMessage;
		LogMessage = logMessage;
	}
}
