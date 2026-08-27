namespace VideoOS.Common.Interfaces.Licensing;

public interface ILicenseReader
{
	string GetSkuFromLicense(string decryptedLicenseData);
}
