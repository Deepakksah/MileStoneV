using System;
using VideoOS.Common.Integration.Command;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
public class DeviceSetupInformationDto
{
	public string DeviceHwid;

	public string DeviceType;

	public string SettingsXml;

	public string PresetsXml;

	public string Streams;

	public static DeviceSetupInformationDto Create(DeviceSetupInformation deviceSetupInformation)
	{
		if (deviceSetupInformation == null)
		{
			throw new ArgumentNullException("deviceSetupInformation");
		}
		return new DeviceSetupInformationDto
		{
			DeviceHwid = deviceSetupInformation.DeviceHwid,
			DeviceType = deviceSetupInformation.DeviceType,
			SettingsXml = deviceSetupInformation.SettingsXml,
			PresetsXml = deviceSetupInformation.PresetsXml,
			Streams = deviceSetupInformation.Streams
		};
	}

	public static DeviceSetupInformation CreateDeviceSetupInformation(DeviceSetupInformationDto deviceSetupInformationDto)
	{
		if (deviceSetupInformationDto == null)
		{
			throw new ArgumentNullException("deviceSetupInformationDto");
		}
		return new DeviceSetupInformation(deviceSetupInformationDto.DeviceHwid, deviceSetupInformationDto.DeviceType, deviceSetupInformationDto.SettingsXml, deviceSetupInformationDto.PresetsXml, deviceSetupInformationDto.Streams);
	}
}
