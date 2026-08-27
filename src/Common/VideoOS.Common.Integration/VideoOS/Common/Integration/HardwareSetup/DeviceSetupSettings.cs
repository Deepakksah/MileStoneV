using System;
using VideoOS.Common.Integration.Definition;

namespace VideoOS.Common.Integration.HardwareSetup;

public class DeviceSetupSettings
{
	public HardwareSetupSettings Hardware { get; private set; }

	public DeviceHwid DeviceHwid { get; private set; }

	public string DeviceType { get; private set; }

	public string SettingsXml { get; private set; }

	public string PresetsXml { get; private set; }

	public string Streams { get; private set; }

	public string InterconnectedProductName
	{
		get
		{
			if (!Hardware.IsIcpOrMipDriver)
			{
				return string.Empty;
			}
			return new DriverSettings(SettingsXml).GetValueByTypeId(DeviceHwid.Text, TypeReferenceIds.DeviceName.ToString());
		}
	}

	public string Name { get; set; }

	public bool Enabled { get; set; }

	public Guid DeviceGroup { get; set; }

	public DeviceSetupSettings(HardwareSetupSettings hardware, DeviceHwid deviceHwid, string deviceType, string settingsXml, string presetsXml, string streams)
	{
		Hardware = hardware;
		DeviceHwid = deviceHwid;
		DeviceType = deviceType;
		SettingsXml = settingsXml;
		PresetsXml = presetsXml;
		Streams = streams;
		Name = string.Empty;
		Enabled = false;
		DeviceGroup = Guid.Empty;
	}
}
