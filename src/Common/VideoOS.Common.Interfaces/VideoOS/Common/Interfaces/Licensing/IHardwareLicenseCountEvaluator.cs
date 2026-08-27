namespace VideoOS.Common.Interfaces.Licensing;

public interface IHardwareLicenseCountEvaluator
{
	string Version { get; }

	bool UpdateLicenseCount(IHardwareInfo hardwareInfo);
}
