using VideoOS.Installer.Configuration;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Manager;
using VideoOS.Installer.UI.Framework;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class LicenseInstallerManager : ILicenseInstallerManager
{
	public LicenseLocationType LicenseLocation
	{
		get
		{
			if (InstallerManager.Instance.InstallerConfig != null && InstallerManager.Instance.InstallerConfig.LicenseProvider != null && InstallerManager.Instance.InstallerConfig.LicenseProvider.LicenseInfo != null)
			{
				return InstallerManager.Instance.InstallerConfig.LicenseProvider.LicenseInfo.LicenseLocation;
			}
			return null;
		}
	}

	public string LicenseType => InstallerManager.Instance.InstallerConfig.LicenseProvider.Type.ToString();

	public string LookupLicenseRegistryEntry()
	{
		return InstallerManager.Instance.LookupLicenseRegistryEntry();
	}

	public void RunCheckLicense(string text, string type, Wizard wizard)
	{
		InstallerManager.Instance.RunCheckLicense(text, type, wizard);
	}

	public void ValidateLicenseStatus(LicenseStatus status)
	{
		InstallerManager.Instance.ValidateStatus(status);
	}
}
