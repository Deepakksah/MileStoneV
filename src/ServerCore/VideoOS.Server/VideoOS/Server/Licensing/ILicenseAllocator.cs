using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Server.Licensing;

public interface ILicenseAllocator
{
	void AllocateLicenses(IHardwareInfo hardwareInfo, RecorderLicense recorderLicense, ILicenseSnapshot licenseSnapshot, bool acquireNewLicenses, bool ignoreAlreadyLicensed);

	void ResetChangesWithoutActivation(int changesWithoutActivation);
}
