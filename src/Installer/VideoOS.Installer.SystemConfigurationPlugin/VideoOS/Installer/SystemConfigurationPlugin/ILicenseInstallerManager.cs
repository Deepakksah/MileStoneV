using VideoOS.Installer.Configuration;
using VideoOS.Installer.UI.Framework;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public interface ILicenseInstallerManager
{
	LicenseLocationType LicenseLocation { get; }

	string LicenseType { get; }

	void RunCheckLicense(string text, string type, Wizard wizard);

	string LookupLicenseRegistryEntry();
}
