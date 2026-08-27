namespace VideoOS.Installer.SystemConfigurationPlugin;

public interface ILicenseHelper
{
	string LookupLicense();

	bool CheckLicense(string license);
}
