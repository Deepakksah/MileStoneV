using System;

namespace VideoOS.Management.Server;

public interface IDriverSettingsComponent : IDisposable
{
	string[] QueryAllSettingsByHardware(string hardwareId);

	HardwareDriverSettingsInfo QueryHardwareDriverSettingsInfo(string hardwareId);

	void UpdateHardwareSettings(string hardwareId, DateTime dateTime, string settingsXml);

	DeviceDriverSettingsInfo QueryDeviceDriverSettingsInfo(string deviceId);

	void UpdateDeviceSettings(string deviceId, DateTime dateTime, string settingsXml, Guid sessionId);
}
