namespace VideoOS.Common.Interfaces.Licensing;

public interface ILicenseImporter
{
	string UpdateLicense(byte[] newLicense);
}
