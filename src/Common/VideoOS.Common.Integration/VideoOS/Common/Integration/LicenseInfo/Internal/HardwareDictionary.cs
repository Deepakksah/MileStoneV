using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Common.Integration.LicenseInfo.Internal;

[Serializable]
public sealed class HardwareDictionary : Dictionary<string, List<IHardwareInfo>>, IHardwareDictionary, IDictionary<string, List<IHardwareInfo>>, ICollection<KeyValuePair<string, List<IHardwareInfo>>>, IEnumerable<KeyValuePair<string, List<IHardwareInfo>>>, IEnumerable
{
	private readonly Dictionary<string, IAggregateFeedDictionary> _hardwareAggregates;

	private readonly Dictionary<string, IAggregateFeedDictionary> _icpHardwareAggregates;

	private bool _hardwareAggregatesUpdated;

	public int InterconnectedCount { get; private set; }

	public int NonInterconnectedCount { get; private set; }

	public List<IHardwareInfo> InterconnectHardware
	{
		get
		{
			List<IHardwareInfo> list = new List<IHardwareInfo>();
			using Enumerator enumerator = GetEnumerator();
			while (enumerator.MoveNext())
			{
				IHardwareInfo hardwareInfo = enumerator.Current.Value.FirstOrDefault((IHardwareInfo h) => h.IsInterconnectedProduct);
				if (hardwareInfo != null)
				{
					list.Add(hardwareInfo);
				}
			}
			return list;
		}
	}

	public List<IHardwareInfo> NonInterconnectHardware
	{
		get
		{
			List<IHardwareInfo> list = new List<IHardwareInfo>();
			using Enumerator enumerator = GetEnumerator();
			while (enumerator.MoveNext())
			{
				IHardwareInfo hardwareInfo = enumerator.Current.Value.FirstOrDefault((IHardwareInfo h) => !h.IsInterconnectedProduct);
				if (hardwareInfo != null)
				{
					list.Add(hardwareInfo);
				}
			}
			return list;
		}
	}

	public HardwareDictionary()
	{
		_hardwareAggregates = new Dictionary<string, IAggregateFeedDictionary>();
		_icpHardwareAggregates = new Dictionary<string, IAggregateFeedDictionary>();
		_hardwareAggregatesUpdated = false;
	}

	private HardwareDictionary(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public void AddHardware(IHardwareInfo hardware)
	{
		if (hardware == null)
		{
			throw new ArgumentNullException("hardware");
		}
		if (hardware.IsInterconnectedProduct)
		{
			InterconnectedCount++;
		}
		else
		{
			NonInterconnectedCount++;
		}
		if (!ContainsKey(hardware.MacAddress))
		{
			Add(hardware.MacAddress, new List<IHardwareInfo>());
		}
		base[hardware.MacAddress].Add(hardware);
	}

	public int GetLicenseCount(string macAddress)
	{
		if (ContainsKey(macAddress))
		{
			return base[macAddress].Sum((IHardwareInfo h) => h.LicenseCount);
		}
		return 0;
	}

	public int GetChannelLicenseCount(string macAddress)
	{
		if (ContainsKey(macAddress))
		{
			return base[macAddress].Sum((IHardwareInfo h) => h.ChannelLicenseCount);
		}
		return 0;
	}

	public int[] EnabledCameraIndexes(string macAddress)
	{
		if (ContainsKey(macAddress))
		{
			List<int> enabledCameraIndexes = new List<int>();
			base[macAddress].ForEach(delegate(IHardwareInfo hardwareInfo)
			{
				enabledCameraIndexes.AddRange(hardwareInfo.EnabledCameraIndexes.Except(enabledCameraIndexes));
			});
			enabledCameraIndexes.Sort();
			return enabledCameraIndexes.ToArray();
		}
		return new int[0];
	}

	public int NumberOfUniqueEnabledCameras(string macAddress)
	{
		return EnabledCameraIndexes(macAddress).Length;
	}

	public void UpdateHardwareAggregates()
	{
		if (_hardwareAggregatesUpdated)
		{
			throw new InvalidOperationException("UpdateHardwareAggregates should only be called once after all hardware is in the HardwareDictionary");
		}
		foreach (IHardwareInfo item in NonInterconnectHardware)
		{
			_hardwareAggregates[item.MacAddress] = GetFeeds(item.MacAddress);
		}
		foreach (IHardwareInfo item2 in InterconnectHardware)
		{
			_icpHardwareAggregates[item2.MacAddress] = GetFeeds(item2.MacAddress);
		}
		_hardwareAggregatesUpdated = true;
	}

	public IAggregateFeedDictionary GetAggregateFeeds(string macAddress)
	{
		ValidateUpdateHardwareAggregatesCalled();
		if (string.IsNullOrWhiteSpace(macAddress))
		{
			throw new ArgumentNullException("macAddress");
		}
		if (!ContainsKey(macAddress))
		{
			throw new ArgumentException("Value of macAddress not found in instance of HardwareDictionary");
		}
		Dictionary<string, IAggregateFeedDictionary> dictionary = ((!base[macAddress][0].IsInterconnectedProduct) ? _hardwareAggregates : _icpHardwareAggregates);
		if (!dictionary.ContainsKey(macAddress))
		{
			throw new ArgumentException("Aggregate dictionary corresponding to value of macAddress not found");
		}
		return dictionary[macAddress];
	}

	public void UpdateTotalFeedCountsAcrossAllHardware(IFeedDictionary allHardwareFeedDictionary)
	{
		ValidateUpdateHardwareAggregatesCalled();
		if (allHardwareFeedDictionary == null)
		{
			throw new ArgumentNullException("allHardwareFeedDictionary");
		}
		foreach (IAggregateFeedDictionary value in _hardwareAggregates.Values)
		{
			foreach (ILicenseFeed value2 in value.Values)
			{
				allHardwareFeedDictionary.IncrementCount(value2.Name, value2.Count);
			}
		}
	}

	public void UpdateTotalICPFeedCountsAcrossAllHardware(IFeedDictionary allHardwareFeedDictionary)
	{
		ValidateUpdateHardwareAggregatesCalled();
		if (allHardwareFeedDictionary == null)
		{
			throw new ArgumentNullException("allHardwareFeedDictionary");
		}
		foreach (IAggregateFeedDictionary value in _icpHardwareAggregates.Values)
		{
			foreach (ILicenseFeed value2 in value.Values)
			{
				allHardwareFeedDictionary.IncrementCount(value2.Name, value2.Count);
			}
		}
	}

	public new void Clear()
	{
		base.Clear();
		_hardwareAggregates.Clear();
		_icpHardwareAggregates.Clear();
		_hardwareAggregatesUpdated = false;
	}

	private void ValidateUpdateHardwareAggregatesCalled()
	{
		if (!_hardwareAggregatesUpdated)
		{
			throw new InvalidOperationException("UpdateHardwareAggregates must be called before performing this operation");
		}
	}

	private IAggregateFeedDictionary GetFeeds(string macAddress)
	{
		if (string.IsNullOrWhiteSpace(macAddress))
		{
			throw new ArgumentNullException("macAddress");
		}
		if (ContainsKey(macAddress))
		{
			return new AggregateFeedDictionary(base[macAddress]);
		}
		return new AggregateFeedDictionary();
	}
}
