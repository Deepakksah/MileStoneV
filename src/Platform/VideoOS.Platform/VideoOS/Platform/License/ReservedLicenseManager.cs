using System;
using System.Globalization;

namespace VideoOS.Platform.License;

public class ReservedLicenseManager
{
	public ReservedLicenseManager()
	{
	}

	public void SaveLicenseInformation(LicenseInformation licenseInformation)
	{
		if (EnvironmentManager.Instance.Mode != Mode.ServerSetup)
		{
			throw new ImplementationMIPException("Only supported in ServerSetup mode");
		}
		if (licenseInformation == null)
		{
			throw new ArgumentNullException("licenseInformation");
		}
		EnvironmentManager.Instance.LicenseManager.SaveLicenseInformation(licenseInformation);
		EnvironmentManager.Instance.Log(error: false, "ReservedLicenseManager", string.Format(CultureInfo.InvariantCulture, "SaveLicenseInformation - Name={0}, Count={1}, Type={2}", licenseInformation.Name, licenseInformation.Counter, licenseInformation.LicenseType));
	}

	public LicenseInformation GetLicenseInformation(Guid pluginId, string licenseType)
	{
		LicenseInformation licenseInformation = EnvironmentManager.Instance.LicenseManager.GetLicenseInformation(pluginId, licenseType);
		if (licenseInformation != null)
		{
			EnvironmentManager.Instance.Log(error: false, "ReservedLicenseManager", string.Format(CultureInfo.InvariantCulture, "GetLicenseInformation - Name={0}, Count={1}, Type={2}", licenseInformation.Name, licenseInformation.Counter, licenseInformation.LicenseType));
		}
		return licenseInformation;
	}
}
