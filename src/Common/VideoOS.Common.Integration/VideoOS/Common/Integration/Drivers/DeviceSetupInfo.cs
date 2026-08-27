using System;
using VideoOS.Common.Integration.Definition;

namespace VideoOS.Common.Integration.Drivers;

public class DeviceSetupInfo : IDeviceSetupInfo
{
	public Guid DeviceId { get; private set; }

	public DeviceHwid Hwid { get; private set; }

	public string SettingsXml { get; private set; }

	public DeviceSetupInfo(Guid deviceId, DeviceHwid hwid, string settingsXml)
	{
		DeviceId = deviceId;
		Hwid = hwid;
		SettingsXml = settingsXml;
	}
}
