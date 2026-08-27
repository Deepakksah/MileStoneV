namespace VideoOS.Common.Interfaces.Licensing;

public interface ILicenseWhitelist
{
	string Version { get; }

	int MaximumChannelCount { get; }

	bool IsHardwareWhitelisted(IHardwareInfo hardwareInfo);
}
