using VideoOS.Platform.License;

namespace VideoOS.IM.Common.Service.Interfaces.Licensing;

public interface ILicenseManager
{
	LicenseInformation LicenseRequest { get; }

	LicenseInformation LicenseInfo { get; }
}
