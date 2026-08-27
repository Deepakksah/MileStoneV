using VideoOS.Common.Integration.LicenseInfo;

namespace VideoOS.Server.Licensing;

public interface ICachedLicenseInfo
{
	LicenseInformation CachedLicenseInfo { get; }

	bool HasCachedLicenseInfo { get; }

	void ClearCachedLicenseInfo();

	void UpdateCachedLicenseInfo(LicenseInformation value);
}
