using System;
using VideoOS.Common.Integration.Definition;

namespace VideoOS.Common.Integration.Drivers;

public interface IDriverDevice : IDisposable
{
	Guid Id { get; }

	int Index { get; }

	DeviceHwid Hwid { get; }

	string DeviceType { get; }

	string SettingsAsXml { get; }

	IDeviceMetaDataLookup MetaDataLookup { get; set; }

	IDeviceLog Log { get; set; }

	event EventHandler<DriverEventArgs> DeviceDriverEvent;

	void Start();

	void Stop();

	void StartFeed();

	void StopFeed();

	void UpdateSettings(string settingsXml);

	string CustomCommandWithResponse(string command, string parameter);
}
