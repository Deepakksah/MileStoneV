using System;
using System.IO;
using VideoOS.Installer.Interfaces;
using VideoOS.Licensing;

namespace VideoOS.Installer.CoLicensePlugin;

public class LicenseProvider : LicenseProviderBase
{
	private const string LicenseFileType = "LicenseFile";

	private const string LicenseKeyType = "LicenseKey";

	private const string LicenseServiceType = "LicenseService";

	public override LicenseStatus CheckLicense(string license, string licenseType, string productKey)
	{
		if (license.Length == 0)
		{
			return LicenseStatus.NotFound;
		}
		return licenseType switch
		{
			"LicenseFile" => CheckLicenseFile(license, productKey), 
			"LicenseKey" => CheckLicenseKey(license), 
			"LicenseService" => CheckLicenseService(license), 
			_ => LicenseStatus.NotFound, 
		};
	}

	public override LicenseStatus CheckLicenseAndGetDisplayName(string license, string licenseType, out string displayName, out string productKey)
	{
		displayName = null;
		productKey = null;
		if (license.Length == 0)
		{
			return LicenseStatus.NotFound;
		}
		return licenseType switch
		{
			"LicenseFile" => CheckLicenseFile(license, out displayName, out productKey), 
			"LicenseKey" => CheckLicenseKey(license), 
			"LicenseService" => CheckLicenseService(license), 
			_ => LicenseStatus.NotFound, 
		};
	}

	private LicenseStatus CheckLicenseFile(string file, out string displayName, out string productKey)
	{
		displayName = null;
		productKey = null;
		if (!string.IsNullOrEmpty(file))
		{
			try
			{
				LicenseChecker licenseChecker = new LicenseChecker(file);
				LicenseStatus num = licenseChecker.Check(string.Empty);
				if (num == LicenseStatus.Ok)
				{
					displayName = licenseChecker.DisplayName;
					productKey = licenseChecker.ProductKey;
				}
				return num;
			}
			catch (FileNotFoundException)
			{
				return LicenseStatus.NotFound;
			}
			catch (LicenseException)
			{
				return LicenseStatus.Invalid;
			}
		}
		return LicenseStatus.Invalid;
	}

	private LicenseStatus CheckLicenseFile(string file, string productKey)
	{
		if (!string.IsNullOrEmpty(file))
		{
			try
			{
				return new LicenseChecker(file).Check(productKey);
			}
			catch (FileNotFoundException)
			{
				return LicenseStatus.NotFound;
			}
			catch (LicenseException)
			{
				return LicenseStatus.Invalid;
			}
		}
		return LicenseStatus.Invalid;
	}

	private LicenseStatus CheckLicenseKey(string key)
	{
		throw new NotSupportedException();
	}

	private LicenseStatus CheckLicenseService(string url)
	{
		throw new NotSupportedException();
	}
}
