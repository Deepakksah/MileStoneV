using System;
using VideoOS.Common.Integration.Definition;

namespace VideoOS.Common.Integration.Drivers;

public interface IDeviceSetupInfo
{
	Guid DeviceId { get; }

	DeviceHwid Hwid { get; }

	string SettingsXml { get; }
}
