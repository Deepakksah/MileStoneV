namespace VideoOS.Installer.Interfaces;

public abstract class LicenseProviderBase : LicenseProviderAbstract
{
	public override LicenseStatus CheckLicense(string license, string licenseType, string productKey)
	{
		return LicenseStatus.Invalid;
	}

	public override LicenseStatus CheckLicenseAndGetDisplayName(string license, string licenseType, out string displayName, out string productKey)
	{
		displayName = string.Empty;
		productKey = string.Empty;
		return LicenseStatus.Invalid;
	}
}
