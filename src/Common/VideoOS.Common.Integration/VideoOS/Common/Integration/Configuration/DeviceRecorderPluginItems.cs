using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace VideoOS.Common.Integration.Configuration;

public class DeviceRecorderPluginItems
{
	private struct DeviceRecorderPluginKey
	{
		private Guid recorderPluginId;

		private Guid deviceId;

		public Guid RecorderPluginId
		{
			[DebuggerStepThrough]
			get
			{
				return recorderPluginId;
			}
		}

		public Guid Deviceid
		{
			[DebuggerStepThrough]
			get
			{
				return deviceId;
			}
		}

		public DeviceRecorderPluginKey(Guid recorderPluginId, Guid deviceId)
		{
			this.recorderPluginId = recorderPluginId;
			this.deviceId = deviceId;
		}

		public override int GetHashCode()
		{
			return recorderPluginId.GetHashCode() + deviceId.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{{RecorderPluginId: {0}; DeviceId: {1}}}", recorderPluginId.ToString(), deviceId.ToString());
		}
	}

	private ConfigurationManager configurationManager;

	private RollbackDictionary<DeviceRecorderPluginKey, DeviceRecorderPlugin> dictionary;

	public DeviceRecorderPluginItems(ConfigurationManager configurationManager)
	{
		this.configurationManager = configurationManager;
		dictionary = new RollbackDictionary<DeviceRecorderPluginKey, DeviceRecorderPlugin>(configurationManager, this);
		foreach (ConfigurationDataSet.RecorderPluginDeviceRow row in this.configurationManager.Dataset.RecorderPluginDevice.Rows)
		{
			AddItem(row);
		}
	}

	public DeviceRecorderPlugin AddNew(Guid IDDevice, Guid IDRecorderPlugin, int processOrder, string activeSettings, bool enabled)
	{
		ConfigurationDataSet.RecorderPluginDeviceRow row = configurationManager.Dataset.RecorderPluginDevice.AddRecorderPluginDeviceRow(DateTime.UtcNow, IDDevice.ToString(), processOrder, IDRecorderPlugin.ToString(), activeSettings, enabled);
		return AddItem(row);
	}

	public bool Contains(Guid recorderPluginID, Guid deviceId)
	{
		try
		{
			return GetDeviceRecorderPlugin(recorderPluginID, deviceId) != null;
		}
		catch (ConfigurationItemNotFoundException)
		{
			return false;
		}
	}

	public void RemoveByDevice(Guid deviceId)
	{
		DeviceRecorderPlugin[] deviceRecorderPluginsByDevice = GetDeviceRecorderPluginsByDevice(deviceId);
		foreach (DeviceRecorderPlugin deviceRecorderPlugin in deviceRecorderPluginsByDevice)
		{
			DeviceRecorderPluginKey key = new DeviceRecorderPluginKey(deviceRecorderPlugin.IDRecorderPlugin, deviceRecorderPlugin.IDDevice);
			deviceRecorderPlugin.DataRow.Delete();
			dictionary.Remove(key);
		}
	}

	private DeviceRecorderPlugin AddItem(ConfigurationDataSet.RecorderPluginDeviceRow row)
	{
		DeviceRecorderPlugin deviceRecorderPlugin = new DeviceRecorderPlugin(configurationManager, row);
		dictionary.Add(new DeviceRecorderPluginKey(deviceRecorderPlugin.IDRecorderPlugin, deviceRecorderPlugin.IDDevice), deviceRecorderPlugin);
		return deviceRecorderPlugin;
	}

	public DeviceRecorderPlugin GetDeviceRecorderPlugin(Guid recorderPluginID, Guid deviceId)
	{
		DeviceRecorderPluginKey key = new DeviceRecorderPluginKey(recorderPluginID, deviceId);
		if (dictionary.TryGetValue(key, out var value))
		{
			return value;
		}
		if (!configurationManager.UseDynamicLoad)
		{
			throw ConfigurationItemNotFoundException.CreateWithKey(typeof(DeviceRecorderPlugin), key.ToString());
		}
		using ConfigurationDataSet configurationDataSet = configurationManager.dataLoader.GetDeviceRecorderPlugin(recorderPluginID, deviceId);
		if (configurationDataSet.RecorderPluginDevice.Rows.Count == 0)
		{
			throw ConfigurationItemNotFoundException.CreateWithKey(typeof(DeviceRecorderPlugin), key.ToString());
		}
		configurationManager.DisableEventHandling = true;
		configurationManager.BeginLoadData(configurationManager.Dataset.RecorderPluginDevice);
		ConfigurationDataSet.RecorderPluginDeviceRow recorderPluginDeviceRow = (ConfigurationDataSet.RecorderPluginDeviceRow)configurationDataSet.RecorderPluginDevice.Rows[0];
		configurationManager.Dataset.RecorderPluginDevice.ImportRow(recorderPluginDeviceRow);
		ConfigurationDataSet.RecorderPluginDeviceRow row = configurationManager.Dataset.RecorderPluginDevice.FindByIDDeviceIDRecorderPlugin(recorderPluginDeviceRow.IDDevice, recorderPluginDeviceRow.IDRecorderPlugin);
		value = AddItem(row);
		configurationManager.EndLoadData(configurationManager.Dataset.RecorderPluginDevice);
		configurationManager.DisableEventHandling = false;
		return value;
	}

	public DeviceRecorderPlugin[] GetDeviceRecorderPluginsByDevice(Guid deviceId)
	{
		List<DeviceRecorderPlugin> list = new List<DeviceRecorderPlugin>();
		if (!configurationManager.UseDynamicLoad)
		{
			foreach (DeviceRecorderPlugin value2 in dictionary.Values)
			{
				if (value2.IDDevice == deviceId)
				{
					list.Add(value2);
				}
			}
		}
		else
		{
			using ConfigurationDataSet configurationDataSet = configurationManager.dataLoader.GetDeviceRecorderPluginsByDeviceId(deviceId);
			configurationManager.DisableEventHandling = true;
			configurationManager.BeginLoadData(configurationManager.Dataset.RecorderPluginDevice);
			foreach (ConfigurationDataSet.RecorderPluginDeviceRow row2 in configurationDataSet.RecorderPluginDevice.Rows)
			{
				DeviceRecorderPluginKey key = new DeviceRecorderPluginKey(new Guid(row2.IDRecorderPlugin), new Guid(row2.IDDevice));
				if (!dictionary.TryGetValue(key, out var value))
				{
					configurationManager.Dataset.RecorderPluginDevice.ImportRow(row2);
					ConfigurationDataSet.RecorderPluginDeviceRow row = configurationManager.Dataset.RecorderPluginDevice.FindByIDDeviceIDRecorderPlugin(row2.IDDevice, row2.IDRecorderPlugin);
					value = AddItem(row);
				}
				list.Add(value);
			}
			configurationManager.EndLoadData(configurationManager.Dataset.RecorderPluginDevice);
			configurationManager.DisableEventHandling = false;
		}
		return list.ToArray();
	}

	public void EnsureRecorderPluginDeviceLoaded(Guid recorderPluginId, Guid[] deviceIds)
	{
		List<Guid> list = new List<Guid>();
		foreach (Guid guid in deviceIds)
		{
			DeviceRecorderPluginKey key = new DeviceRecorderPluginKey(recorderPluginId, guid);
			if (!dictionary.TryGetValue(key, out var _))
			{
				list.Add(guid);
			}
		}
		if (list.Count <= 0)
		{
			return;
		}
		if (!configurationManager.UseDynamicLoad)
		{
			throw ConfigurationItemNotFoundException.Create(typeof(DeviceRecorderPlugin));
		}
		int num = 250;
		for (int j = 0; j < list.Count; j += num)
		{
			int num2 = list.Count - j;
			int num3 = ((num2 > num) ? num : num2);
			Guid[] array = new Guid[num3];
			list.CopyTo(j, array, 0, num3);
			using ConfigurationDataSet configurationDataSet = configurationManager.dataLoader.GetDeviceRecorderPlugins(recorderPluginId, array);
			if (configurationDataSet.RecorderPluginDevice.Rows.Count != num3)
			{
				throw ConfigurationItemNotFoundException.Create(typeof(DeviceRecorderPlugin));
			}
			configurationManager.DisableEventHandling = true;
			configurationManager.BeginLoadData(configurationManager.Dataset.RecorderPluginDevice);
			foreach (ConfigurationDataSet.RecorderPluginDeviceRow row2 in configurationDataSet.RecorderPluginDevice.Rows)
			{
				configurationManager.Dataset.RecorderPluginDevice.ImportRow(row2);
				ConfigurationDataSet.RecorderPluginDeviceRow row = configurationManager.Dataset.RecorderPluginDevice.FindByIDDeviceIDRecorderPlugin(row2.IDDevice, row2.IDRecorderPlugin);
				AddItem(row);
			}
			configurationManager.EndLoadData(configurationManager.Dataset.RecorderPluginDevice);
			configurationManager.DisableEventHandling = false;
		}
	}
}
