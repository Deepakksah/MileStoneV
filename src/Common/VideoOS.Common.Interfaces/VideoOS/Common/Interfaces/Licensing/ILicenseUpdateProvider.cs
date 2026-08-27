namespace VideoOS.Common.Interfaces.Licensing;

public interface ILicenseUpdateProvider
{
	void UpdateLicense(byte[] license);
}
