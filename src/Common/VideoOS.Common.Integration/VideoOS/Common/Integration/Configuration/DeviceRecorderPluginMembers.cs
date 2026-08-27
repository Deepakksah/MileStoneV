using System;

namespace VideoOS.Common.Integration.Configuration;

public class DeviceRecorderPluginMembers
{
	private ConfigurationManager configurationManager;

	private Device device;

	public DeviceRecorderPlugin this[Guid recorderPluginID] => configurationManager.DeviceRecorderPluginItems.GetDeviceRecorderPlugin(recorderPluginID, device.ID);

	public DeviceRecorderPluginMembers(ConfigurationManager manager, Device device)
	{
		configurationManager = manager;
		this.device = device;
	}

	public bool Contains(Guid recorderPluginID)
	{
		return configurationManager.DeviceRecorderPluginItems.Contains(recorderPluginID, device.ID);
	}

	public DeviceRecorderPlugin Add(Guid recorderPluginID, string activeSettings, bool enabled)
	{
		DeviceRecorderPlugin[] deviceRecorderPluginsByDevice = configurationManager.DeviceRecorderPluginItems.GetDeviceRecorderPluginsByDevice(device.ID);
		int num = 0;
		DeviceRecorderPlugin[] array = deviceRecorderPluginsByDevice;
		foreach (DeviceRecorderPlugin deviceRecorderPlugin in array)
		{
			if (num <= deviceRecorderPlugin.ProcessOrder)
			{
				num = deviceRecorderPlugin.ProcessOrder + 1;
			}
		}
		return configurationManager.DeviceRecorderPluginItems.AddNew(device.ID, recorderPluginID, num, activeSettings, enabled);
	}

	public void Clear()
	{
		configurationManager.DeviceRecorderPluginItems.RemoveByDevice(device.ID);
	}
}
