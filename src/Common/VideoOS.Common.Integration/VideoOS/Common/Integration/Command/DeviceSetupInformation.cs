using System;

namespace VideoOS.Common.Integration.Command;

[Serializable]
public class DeviceSetupInformation
{
	public string DeviceHwid { get; private set; }

	public string DeviceType { get; private set; }

	public string SettingsXml { get; private set; }

	public string PresetsXml { get; private set; }

	public string Streams { get; private set; }

	public DeviceSetupInformation(string deviceHwid, string deviceType, string settingsXml, string presetsXml, string streams)
	{
		DeviceHwid = deviceHwid;
		DeviceType = deviceType;
		SettingsXml = settingsXml;
		PresetsXml = presetsXml;
		Streams = streams;
	}
}
