using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace VideoOS.Common.Integration.Configuration;

public class DevicePrivacyMaskItems
{
	private struct DevicePrivacyMaskKey
	{
		public Guid DeviceId { get; private set; }

		public DateTime ModifiedTime { get; private set; }

		public DevicePrivacyMaskKey(Guid deviceId, DateTime modifiedTime)
		{
			DeviceId = deviceId;
			ModifiedTime = modifiedTime;
		}

		public override int GetHashCode()
		{
			return DeviceId.GetHashCode() + ModifiedTime.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{{DeviceId: {0}; Time: {1}}}", DeviceId.ToString(), ModifiedTime.ToString(CultureInfo.InvariantCulture));
		}
	}

	private class DevicePrivacyMask : IConfigurationItemRow
	{
		private ConfigurationDataSet.DevicePrivacyMaskRow datarow;

		public DataRow DataRow
		{
			[DebuggerStepThrough]
			get
			{
				return datarow;
			}
		}

		public Guid IDDevice
		{
			[DebuggerStepThrough]
			get
			{
				return new Guid(datarow.IDDevice);
			}
		}

		public DateTime ModifiedTime
		{
			[DebuggerStepThrough]
			get
			{
				return datarow.LastModified;
			}
		}

		public DevicePrivacyMask(ConfigurationDataSet.DevicePrivacyMaskRow row)
		{
			datarow = row;
		}
	}

	private DateTime initialTime = DateTime.Parse("1980-01-01T10:00:00Z", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);

	private ConfigurationManager configurationManager;

	private RollbackDictionary<DevicePrivacyMaskKey, DevicePrivacyMask> dictionary;

	public DevicePrivacyMaskItems(ConfigurationManager configurationManager)
	{
		this.configurationManager = configurationManager;
		dictionary = new RollbackDictionary<DevicePrivacyMaskKey, DevicePrivacyMask>(configurationManager, this);
		foreach (ConfigurationDataSet.DevicePrivacyMaskRow row in this.configurationManager.Dataset.DevicePrivacyMask.Rows)
		{
			AddItem(row);
		}
	}

	public void AddNew(Guid deviceId)
	{
		ConfigurationDataSet.DevicePrivacyMaskRow row = configurationManager.Dataset.DevicePrivacyMask.AddDevicePrivacyMaskRow(initialTime, deviceId.ToString(), string.Empty, Enabled: false);
		AddItem(row);
	}

	private DevicePrivacyMask AddItem(ConfigurationDataSet.DevicePrivacyMaskRow row)
	{
		DevicePrivacyMask devicePrivacyMask = new DevicePrivacyMask(row);
		dictionary.Add(new DevicePrivacyMaskKey(devicePrivacyMask.IDDevice, devicePrivacyMask.ModifiedTime), devicePrivacyMask);
		return devicePrivacyMask;
	}

	public void RemoveByDevice(Guid deviceId)
	{
		foreach (DevicePrivacyMask devicePrivacyMask in GetDevicePrivacyMasks(deviceId))
		{
			DevicePrivacyMaskKey key = new DevicePrivacyMaskKey(devicePrivacyMask.IDDevice, devicePrivacyMask.ModifiedTime);
			devicePrivacyMask.DataRow.Delete();
			dictionary.Remove(key);
		}
	}

	private IEnumerable<DevicePrivacyMask> GetDevicePrivacyMasks(Guid deviceId)
	{
		List<DevicePrivacyMask> list = new List<DevicePrivacyMask>();
		IEnumerable<DevicePrivacyMask> enumerable = from kvp in dictionary
			where kvp.Key.DeviceId == deviceId
			select kvp.Value;
		if (enumerable.Any())
		{
			list.AddRange(enumerable);
			return list;
		}
		if (!configurationManager.UseDynamicLoad)
		{
			return list;
		}
		using ConfigurationDataSet configurationDataSet = configurationManager.dataLoader.GetDevicePrivacyMaskByDeviceIds(new Guid[1] { deviceId });
		configurationManager.DisableEventHandling = true;
		configurationManager.BeginLoadData(configurationManager.Dataset.DevicePrivacyMask);
		foreach (ConfigurationDataSet.DevicePrivacyMaskRow row2 in configurationDataSet.DevicePrivacyMask.Rows)
		{
			new DevicePrivacyMaskKey(new Guid(row2.IDDevice), row2.LastModified);
			configurationManager.Dataset.DevicePrivacyMask.ImportRow(row2);
			ConfigurationDataSet.DevicePrivacyMaskRow row = configurationManager.Dataset.DevicePrivacyMask.FindByLastModifiedIDDevice(row2.LastModified, row2.IDDevice);
			DevicePrivacyMask item = AddItem(row);
			list.Add(item);
		}
		configurationManager.EndLoadData(configurationManager.Dataset.DevicePrivacyMask);
		configurationManager.DisableEventHandling = false;
		return list;
	}

	public void EnsureDevicePrivacyMaskItemsLoaded(Guid[] deviceIds)
	{
		List<Guid> list = new List<Guid>(deviceIds);
		if (list.Count <= 0)
		{
			return;
		}
		if (!configurationManager.UseDynamicLoad)
		{
			throw ConfigurationItemNotFoundException.Create(typeof(DevicePrivacyMask));
		}
		int num = 400;
		for (int i = 0; i < list.Count; i += num)
		{
			int num2 = list.Count - i;
			int num3 = ((num2 > num) ? num : num2);
			Guid[] array = new Guid[num3];
			list.CopyTo(i, array, 0, num3);
			using ConfigurationDataSet configurationDataSet = configurationManager.dataLoader.GetDevicePrivacyMaskByDeviceIds(array);
			configurationManager.DisableEventHandling = true;
			configurationManager.BeginLoadData(configurationManager.Dataset.DevicePrivacyMask);
			foreach (ConfigurationDataSet.DevicePrivacyMaskRow row2 in configurationDataSet.DevicePrivacyMask.Rows)
			{
				DevicePrivacyMaskKey key = new DevicePrivacyMaskKey(new Guid(row2.IDDevice), row2.LastModified);
				if (!dictionary.ContainsKey(key))
				{
					configurationManager.Dataset.DevicePrivacyMask.ImportRow(row2);
					ConfigurationDataSet.DevicePrivacyMaskRow row = configurationManager.Dataset.DevicePrivacyMask.FindByLastModifiedIDDevice(row2.LastModified, row2.IDDevice);
					AddItem(row);
				}
			}
			configurationManager.EndLoadData(configurationManager.Dataset.DevicePrivacyMask);
			configurationManager.DisableEventHandling = false;
		}
	}
}
