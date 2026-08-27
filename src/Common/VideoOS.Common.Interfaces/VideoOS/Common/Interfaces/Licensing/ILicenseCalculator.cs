using System.Collections.Generic;

namespace VideoOS.Common.Interfaces.Licensing;

public interface ILicenseCalculator
{
	void Initialize(ILicenseDocument licenseDocument);

	bool CalculateLicenseRequirements(IEnumerable<IHardwareInfo> hardwareInfoList, out List<ILicenseCalculationResult> licenseCalculations);

	ILicenseCalculationResult CalculateLicenseRequirementsForHardware(IHardwareInfo hardwareInfo);
}
