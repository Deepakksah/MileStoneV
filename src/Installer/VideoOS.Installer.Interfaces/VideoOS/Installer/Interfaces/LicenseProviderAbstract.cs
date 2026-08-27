namespace VideoOS.Installer.Interfaces;

public abstract class LicenseProviderAbstract : ILicenseProvider
{
	public abstract LicenseStatus CheckLicense(string license, string licenseType, string productKey);

	public abstract LicenseStatus CheckLicenseAndGetDisplayName(string license, string licenseType, out string displayName, out string productKey);
}
