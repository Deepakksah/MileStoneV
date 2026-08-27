using System;
using System.IO;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Manager;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class LicenseHelper : ILicenseHelper
{
	private ILicenseInstallerManager _installerManager;

	public LicenseHelper()
		: this(new LicenseInstallerManager())
	{
	}

	public LicenseHelper(ILicenseInstallerManager installerManager)
	{
		if (installerManager == null)
		{
			throw new ArgumentNullException("installerManager");
		}
		_installerManager = installerManager;
	}

	public bool CheckLicense(string license)
	{
		if (string.IsNullOrEmpty(license))
		{
			return false;
		}
		try
		{
			_installerManager.RunCheckLicense(license, _installerManager.LicenseType, null);
			return true;
		}
		catch (LicenseCheckException)
		{
			return false;
		}
	}

	public string LookupLicense()
	{
		string text = null;
		if (ValidateLicenseLocation(_installerManager.LicenseLocation))
		{
			if (_installerManager.LicenseLocation.Type == LicenseLocationTypeType.Registry)
			{
				text = RegistryKeyHelper.GetLocalMachineValue(_installerManager.LicenseLocation.Path, _installerManager.LicenseLocation.Name);
			}
			else if (_installerManager.LicenseLocation.Type == LicenseLocationTypeType.File)
			{
				text = Path.Combine(_installerManager.LicenseLocation.Path, _installerManager.LicenseLocation.Name);
			}
		}
		if (string.IsNullOrEmpty(text))
		{
			text = _installerManager.LookupLicenseRegistryEntry();
		}
		return text;
	}

	private static bool ValidateLicenseLocation(LicenseLocationType licenseLocation)
	{
		if (licenseLocation == null)
		{
			return false;
		}
		if (string.IsNullOrEmpty(licenseLocation.Path))
		{
			LogManager.Write("Path in license info is empty");
		}
		else
		{
			if (!string.IsNullOrEmpty(licenseLocation.Name))
			{
				return true;
			}
			LogManager.Write("Name in license info is empty");
		}
		return false;
	}
}
