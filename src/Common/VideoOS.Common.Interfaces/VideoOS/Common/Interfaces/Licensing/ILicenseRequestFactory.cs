namespace VideoOS.Common.Interfaces.Licensing;

public interface ILicenseRequestFactory
{
	ILicenseRequest CreateLicenseRequest(bool includeHardwareIds);
}
