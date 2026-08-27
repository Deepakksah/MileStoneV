using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Common.Integration.Configuration;

public class DeviceDataDistributionCollection : IEnumerable<DeviceDataDistributionEntry>, IEnumerable
{
	private struct DeviceDataDistributionKey(Guid recorderId, Guid trackId)
	{
		private Guid RecorderId = recorderId;

		private Guid TrackId = trackId;

		public override int GetHashCode()
		{
			return (RecorderId.GetHashCode() * 397) ^ TrackId.GetHashCode();
		}
	}

	private ConfigurationManager configurationManager;

	private RollbackDictionary<DeviceDataDistributionKey, DeviceDataDistributionEntry> dictionary;

	public DeviceDataDistributionCollection(ConfigurationManager configurationManager)
	{
		if (configurationManager == null)
		{
			throw new ArgumentNullException("configurationManager");
		}
		this.configurationManager = configurationManager;
		dictionary = new RollbackDictionary<DeviceDataDistributionKey, DeviceDataDistributionEntry>(this.configurationManager, this);
		ConfigurationDataSet.DeviceDataDistributionDataTable deviceDataDistribution = configurationManager.Dataset.DeviceDataDistribution;
		this.configurationManager.EnsureTableDataLoaded(deviceDataDistribution);
		foreach (ConfigurationDataSet.DeviceDataDistributionRow item in deviceDataDistribution)
		{
			DeviceDataDistributionEntry deviceDataDistributionEntry = new DeviceDataDistributionEntry(this.configurationManager, item);
			DeviceDataDistributionKey key = new DeviceDataDistributionKey(deviceDataDistributionEntry.RecorderId, deviceDataDistributionEntry.TrackId);
			dictionary.Add(key, deviceDataDistributionEntry);
		}
	}

	public void Remove(DeviceDataDistributionEntry item)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		DeviceDataDistributionKey key = new DeviceDataDistributionKey(item.RecorderId, item.TrackId);
		dictionary.Remove(key);
		item.DataRow.Delete();
	}

	public void RemoveByDevice(Device device)
	{
		if (device == null)
		{
			throw new ArgumentNullException("device");
		}
		DeviceDataDistributionEntry[] array = this.Where((DeviceDataDistributionEntry i) => i.DeviceId == device.ID).ToArray();
		foreach (DeviceDataDistributionEntry item in array)
		{
			Remove(item);
		}
	}

	public void RemoveByRecorder(Recorder recorder)
	{
		if (recorder == null)
		{
			throw new ArgumentNullException("recorder");
		}
		DeviceDataDistributionEntry[] array = this.Where((DeviceDataDistributionEntry i) => i.RecorderId == recorder.ID).ToArray();
		foreach (DeviceDataDistributionEntry item in array)
		{
			Remove(item);
		}
	}

	public IEnumerator<DeviceDataDistributionEntry> GetEnumerator()
	{
		return dictionary.Values.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return dictionary.Values.GetEnumerator();
	}
}
