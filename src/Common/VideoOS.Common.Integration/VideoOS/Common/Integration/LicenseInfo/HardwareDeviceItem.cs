using System;
using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Common.Integration.LicenseInfo;

public sealed class HardwareDeviceItem : IHardwareDeviceItem
{
	public string MacAddress { get; private set; }

	public string DeviceId { get; private set; }

	public HardwareDeviceItem(string macAddress, string deviceId)
	{
		if (string.IsNullOrEmpty(macAddress))
		{
			throw new ArgumentNullException("macAddress");
		}
		if (string.IsNullOrEmpty(deviceId))
		{
			throw new ArgumentNullException("deviceId");
		}
		MacAddress = macAddress;
		DeviceId = deviceId;
	}
}
