using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public interface IDeviceRecorderPluginDataComponent : IDisposable
{
	IEnumerable<DeviceRecorderPlugin> QueryEnabledPluginsByType(Guid pluginTypeId);

	DeviceRecorderPlugin QueryEnabledPluginsByTypeAndId(Guid pluginTypeId, Guid deviceId);

	void UpdateDeviceFisheyeSettings(Guid deviceId, string settings, bool enabled, Guid sessionId);
}
