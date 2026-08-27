namespace VideoOS.Installer.Interfaces;

public interface ILicenseProvider
{
	LicenseStatus CheckLicense(string license, string licenseType, string productKey);

	LicenseStatus CheckLicenseAndGetDisplayName(string license, string licenseType, out string displayName, out string productKey);
}
