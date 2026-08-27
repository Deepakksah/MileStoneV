namespace VideoOS.Common.Interfaces.Licensing;

public interface ILicenseLoader
{
	ILicenseDocument CreateLicenseDocument(string plainTextLicense);

	ILicenseDocument CreateLicenseDocument(byte[] utf8LicenseBytes);

	byte[] ConvertLicenseForStorage(byte[] licenseForStorage);

	ILicenseDocument ImportLicenseFile(byte[] externallyEncryptedLicense);

	ILicenseDocument ImportLicenseFile(string fileName);
}
