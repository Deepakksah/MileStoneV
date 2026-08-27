using System;
using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Common.Integration.LicenseInfo;

public class LicenseCalculationResult : ILicenseCalculationResult
{
	public string MacAddress { get; private set; }

	public string ModelName { get; private set; }

	public string DriverName { get; private set; }

	public string HardwareName { get; private set; }

	public int ChannelLicenses { get; private set; }

	public LicenseCalculationResult(string macAddress, string modelName, string driverName, string hardwareName, int channelLicenses)
	{
		if (string.IsNullOrEmpty(macAddress))
		{
			throw new ArgumentNullException("macAddress");
		}
		if (channelLicenses < 0)
		{
			throw new ArgumentOutOfRangeException("channelLicenses");
		}
		if (modelName == null)
		{
			modelName = string.Empty;
		}
		if (driverName == null)
		{
			driverName = string.Empty;
		}
		if (hardwareName == null)
		{
			hardwareName = string.Empty;
		}
		MacAddress = macAddress;
		ModelName = modelName;
		DriverName = driverName;
		HardwareName = hardwareName;
		ChannelLicenses = channelLicenses;
	}
}
