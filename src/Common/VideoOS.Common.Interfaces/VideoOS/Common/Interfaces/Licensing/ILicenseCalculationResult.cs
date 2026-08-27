namespace VideoOS.Common.Interfaces.Licensing;

public interface ILicenseCalculationResult
{
	string MacAddress { get; }

	string ModelName { get; }

	string DriverName { get; }

	string HardwareName { get; }

	int ChannelLicenses { get; }
}
