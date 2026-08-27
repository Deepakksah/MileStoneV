using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace VideoOS.Common.Integration.Configuration;

public class DeviceTrackItems
{
	private struct DeviceTrackKey
	{
		public Guid DeviceId { get; private set; }

		public Guid TrackId { get; private set; }

		public DeviceTrackKey(Guid deviceId, Guid trackId)
		{
			DeviceId = deviceId;
			TrackId = trackId;
		}

		public override int GetHashCode()
		{
			return DeviceId.GetHashCode() + TrackId.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{{DeviceId: {0}; TrackId: {1}}}", DeviceId.ToString(), TrackId.ToString());
		}
	}

	private ConfigurationManager configurationManager;

	private RollbackDictionary<DeviceTrackKey, DeviceTrack> dictionary;

	public DeviceTrackItems(ConfigurationManager configurationManager)
	{
		this.configurationManager = configurationManager;
		dictionary = new RollbackDictionary<DeviceTrackKey, DeviceTrack>(configurationManager, this);
		foreach (ConfigurationDataSet.DeviceTrackRow row in this.configurationManager.Dataset.DeviceTrack.Rows)
		{
			AddItem(row);
		}
	}

	public void AddNew(Guid deviceId)
	{
		DateTime utcNow = DateTime.UtcNow;
		ConfigurationDataSet.DeviceTrackRow row = configurationManager.Dataset.DeviceTrack.AddDeviceTrackRow(deviceId.ToString(), deviceId, WellKnownDeviceTrackDefinition.Primary, MergeEdge: true, DefaultPlayback: true, InUse: true, utcNow, utcNow);
		AddItem(row);
		row = configurationManager.Dataset.DeviceTrack.AddDeviceTrackRow(deviceId.ToString(), Guid.NewGuid(), WellKnownDeviceTrackDefinition.Edge, MergeEdge: false, DefaultPlayback: false, InUse: true, utcNow, utcNow);
		AddItem(row);
	}

	public DeviceTrack AddNew(Guid deviceId, Guid trackDefinitionId, bool defaultPlayback, bool mergeEdge, bool inUse)
	{
		DateTime utcNow = DateTime.UtcNow;
		ConfigurationDataSet.DeviceTrackRow row = configurationManager.Dataset.DeviceTrack.AddDeviceTrackRow(deviceId.ToString(), Guid.NewGuid(), trackDefinitionId, mergeEdge, defaultPlayback, inUse, utcNow, utcNow);
		return AddItem(row);
	}

	private DeviceTrack AddItem(ConfigurationDataSet.DeviceTrackRow row)
	{
		DeviceTrack deviceTrack = new DeviceTrack(row);
		dictionary.Add(new DeviceTrackKey(deviceTrack.IDDevice, deviceTrack.IDTrack), deviceTrack);
		return deviceTrack;
	}

	public void RemoveByDevice(Guid deviceId)
	{
		foreach (DeviceTrack deviceTrack in GetDeviceTracks(deviceId))
		{
			DeviceTrackKey key = new DeviceTrackKey(deviceTrack.IDDevice, deviceTrack.IDTrack);
			deviceTrack.DataRow.Delete();
			dictionary.Remove(key);
		}
	}

	public IEnumerable<DeviceTrack> GetDeviceTracks(Guid deviceId)
	{
		List<DeviceTrack> list = new List<DeviceTrack>();
		IEnumerable<DeviceTrack> enumerable = from kvp in dictionary
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
		using ConfigurationDataSet configurationDataSet = configurationManager.dataLoader.GetDeviceTracksByDeviceIds(new Guid[1] { deviceId });
		configurationManager.DisableEventHandling = true;
		configurationManager.BeginLoadData(configurationManager.Dataset.DeviceTrack);
		foreach (ConfigurationDataSet.DeviceTrackRow row2 in configurationDataSet.DeviceTrack.Rows)
		{
			new DeviceTrackKey(new Guid(row2.IDDevice), row2.IDTrack);
			configurationManager.Dataset.DeviceTrack.ImportRow(row2);
			ConfigurationDataSet.DeviceTrackRow row = configurationManager.Dataset.DeviceTrack.FindByIDTrack(row2.IDTrack);
			DeviceTrack item = AddItem(row);
			list.Add(item);
		}
		configurationManager.EndLoadData(configurationManager.Dataset.DeviceTrack);
		configurationManager.DisableEventHandling = false;
		return list;
	}

	public void EnsureDeviceTrackItemsLoaded(IEnumerable<Guid> deviceIds)
	{
		List<Guid> list = new List<Guid>(deviceIds);
		if (list.Count <= 0)
		{
			return;
		}
		if (!configurationManager.UseDynamicLoad)
		{
			throw ConfigurationItemNotFoundException.Create(typeof(DeviceTrack));
		}
		int num = 400;
		for (int i = 0; i < list.Count; i += num)
		{
			int num2 = list.Count - i;
			int num3 = ((num2 > num) ? num : num2);
			Guid[] array = new Guid[num3];
			list.CopyTo(i, array, 0, num3);
			using ConfigurationDataSet configurationDataSet = configurationManager.dataLoader.GetDeviceTracksByDeviceIds(array);
			configurationManager.DisableEventHandling = true;
			configurationManager.BeginLoadData(configurationManager.Dataset.DeviceTrack);
			foreach (ConfigurationDataSet.DeviceTrackRow row2 in configurationDataSet.DeviceTrack.Rows)
			{
				DeviceTrackKey key = new DeviceTrackKey(new Guid(row2.IDDevice), row2.IDTrack);
				if (!dictionary.ContainsKey(key))
				{
					configurationManager.Dataset.DeviceTrack.ImportRow(row2);
					ConfigurationDataSet.DeviceTrackRow row = configurationManager.Dataset.DeviceTrack.FindByIDTrack(row2.IDTrack);
					AddItem(row);
				}
			}
			configurationManager.EndLoadData(configurationManager.Dataset.DeviceTrack);
			configurationManager.DisableEventHandling = false;
		}
	}
}
