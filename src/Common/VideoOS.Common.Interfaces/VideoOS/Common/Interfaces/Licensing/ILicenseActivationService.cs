namespace VideoOS.Common.Interfaces.Licensing;

public interface ILicenseActivationService
{
	byte[] AcquireLicense(string userName, string password, byte[] request);
}
