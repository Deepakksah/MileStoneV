using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Common.Integration.Configuration;

public class DeviceItems
{
	private ConfigurationManager configurationManager;

	private RollbackDictionary<Guid, Device> dictionary;

	public DeviceItems(ConfigurationManager configurationManager)
	{
		this.configurationManager = configurationManager;
		dictionary = new RollbackDictionary<Guid, Device>(configurationManager, this);
		foreach (ConfigurationDataSet.DevicesRow row in this.configurationManager.Dataset.Devices.Rows)
		{
			AddItem(row);
		}
	}

	public Device AddNew(Guid id, string name, string description, string location, string deviceType, bool enabled, IPHardware hardware, string settings, string presets, string shortcut, bool multicastEnabled, string streams, int deviceIndex, bool failoverLiveOnly, bool disableOnFailover, int ptzManualSessionTimeout, int ptzReservedSessionTimeout, int ptzExtendedManualSessionTimeout, string gisPointString, string shortName, int icon, double coverageDirection, double coverageDepth, double coverageFieldOfView)
	{
		ConfigurationDataSet.DevicesRow row = configurationManager.Dataset.Devices.AddDevicesRow(DateTime.UtcNow, id.ToString(), hardware.ID.ToString(), name, description, location, deviceType, enabled, settings, presets, shortcut, multicastEnabled, streams, deviceIndex, (int)hardware.Subtype, failoverLiveOnly, disableOnFailover, ptzManualSessionTimeout, ptzReservedSessionTimeout, ptzExtendedManualSessionTimeout, gisPointString, shortName, icon, coverageDirection, coverageDepth, coverageFieldOfView);
		return AddItem(row);
	}

	public bool TryGetDevice(Guid id, out Device device)
	{
		return dictionary.TryGetValue(id, out device);
	}

	public Device GetDeviceByDeviceId(Guid id)
	{
		if (dictionary.TryGetValue(id, out var value))
		{
			return value;
		}
		if (!configurationManager.UseDynamicLoad)
		{
			throw ConfigurationItemNotFoundException.CreateWithId(typeof(Device), id);
		}
		using ConfigurationDataSet configurationDataSet = configurationManager.dataLoader.GetDeviceByDeviceId(id);
		if (configurationDataSet.Devices.Rows.Count == 0)
		{
			throw ConfigurationItemNotFoundException.CreateWithId(typeof(Device), id);
		}
		configurationManager.DisableEventHandling = true;
		configurationManager.BeginLoadData(configurationManager.Dataset.Devices);
		ConfigurationDataSet.DevicesRow devicesRow = (ConfigurationDataSet.DevicesRow)configurationDataSet.Devices.Rows[0];
		configurationManager.Dataset.Devices.ImportRow(devicesRow);
		ConfigurationDataSet.DevicesRow row = configurationManager.Dataset.Devices.FindByIDDevice(devicesRow.IDDevice);
		value = AddItem(row);
		configurationManager.EndLoadData(configurationManager.Dataset.Devices);
		configurationManager.DisableEventHandling = false;
		return value;
	}

	public Device[] GetDevicesByHardwareId(Guid hardwareId, bool useAlreadyLoaded)
	{
		List<Device> list = new List<Device>();
		if (!configurationManager.UseDynamicLoad || useAlreadyLoaded)
		{
			foreach (Device value2 in dictionary.Values)
			{
				if (value2.IPHardwareID == hardwareId)
				{
					list.Add(value2);
				}
			}
		}
		else
		{
			using ConfigurationDataSet configurationDataSet = configurationManager.dataLoader.GetDevicesByHardwareId(hardwareId);
			configurationManager.DisableEventHandling = true;
			configurationManager.BeginLoadData(configurationManager.Dataset.Devices);
			foreach (ConfigurationDataSet.DevicesRow row2 in configurationDataSet.Devices.Rows)
			{
				Guid key = new Guid(row2.IDDevice);
				if (!dictionary.TryGetValue(key, out var value))
				{
					configurationManager.Dataset.Devices.ImportRow(row2);
					ConfigurationDataSet.DevicesRow row = configurationManager.Dataset.Devices.FindByIDDevice(row2.IDDevice);
					value = AddItem(row);
				}
				list.Add(value);
			}
			configurationManager.EndLoadData(configurationManager.Dataset.Devices);
			configurationManager.DisableEventHandling = false;
		}
		return list.ToArray();
	}

	public void LoadDevicesByRecorderId(Guid recorderId)
	{
		using ConfigurationDataSet configurationDataSet = configurationManager.dataLoader.GetDevicesByRecorderId(recorderId);
		configurationManager.DisableEventHandling = true;
		configurationManager.BeginLoadData(configurationManager.Dataset.Devices);
		foreach (ConfigurationDataSet.DevicesRow row2 in configurationDataSet.Devices.Rows)
		{
			Guid key = new Guid(row2.IDDevice);
			if (!dictionary.TryGetValue(key, out var _))
			{
				configurationManager.Dataset.Devices.ImportRow(row2);
				ConfigurationDataSet.DevicesRow row = configurationManager.Dataset.Devices.FindByIDDevice(row2.IDDevice);
				AddItem(row);
			}
		}
		configurationManager.EndLoadData(configurationManager.Dataset.Devices);
		configurationManager.DisableEventHandling = false;
	}

	public void Remove(Guid id)
	{
		Device deviceByDeviceId = GetDeviceByDeviceId(id);
		if (deviceByDeviceId != null)
		{
			dictionary.Remove(id);
			deviceByDeviceId.DataRow.Delete();
		}
	}

	private Device AddItem(ConfigurationDataSet.DevicesRow row)
	{
		Device device = new Device(configurationManager, row);
		dictionary.Add(device.ID, device);
		return device;
	}

	public Device[] GetDevicesByDeviceIds(IEnumerable<Guid> deviceIds, bool supressItemNotFoundError)
	{
		List<Device> list = new List<Device>(deviceIds.Count());
		List<Guid> list2 = new List<Guid>();
		foreach (Guid deviceId in deviceIds)
		{
			if (dictionary.TryGetValue(deviceId, out var value))
			{
				list.Add(value);
			}
			else
			{
				list2.Add(deviceId);
			}
		}
		if (list2.Count > 0)
		{
			if (!configurationManager.UseDynamicLoad && !supressItemNotFoundError)
			{
				throw ConfigurationItemNotFoundException.Create(typeof(Device));
			}
			int num = 200;
			for (int i = 0; i < list2.Count; i += num)
			{
				int num2 = list2.Count - i;
				int num3 = ((num2 > num) ? num : num2);
				Guid[] array = new Guid[num3];
				list2.CopyTo(i, array, 0, num3);
				using ConfigurationDataSet configurationDataSet = configurationManager.dataLoader.GetDevicesByDeviceIds(array);
				if (configurationDataSet.Devices.Rows.Count != num3 && !supressItemNotFoundError)
				{
					throw ConfigurationItemNotFoundException.Create(typeof(Device));
				}
				configurationManager.DisableEventHandling = true;
				configurationManager.BeginLoadData(configurationManager.Dataset.Devices);
				foreach (ConfigurationDataSet.DevicesRow row2 in configurationDataSet.Devices.Rows)
				{
					configurationManager.Dataset.Devices.ImportRow(row2);
					ConfigurationDataSet.DevicesRow row = configurationManager.Dataset.Devices.FindByIDDevice(row2.IDDevice);
					Device item = AddItem(row);
					list.Add(item);
				}
				configurationManager.EndLoadData(configurationManager.Dataset.Devices);
				configurationManager.DisableEventHandling = false;
			}
		}
		return list.ToArray();
	}
}
