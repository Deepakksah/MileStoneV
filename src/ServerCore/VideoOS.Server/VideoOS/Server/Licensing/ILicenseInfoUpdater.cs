using System;
using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Server.Licensing;

public interface ILicenseInfoUpdater
{
	void UpdateLicenseInformation(Guid recorderId, IHardwareLicenseCountEvaluator hardwareLicenseCountEvaluator, ILicenseSnapshot licenseSnapshot, IHardwareInfo hardwareInfo);
}
