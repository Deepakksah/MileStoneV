using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using VideoOS.Common.Integration.LicenseInfo.Internal;
using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Common.Integration.LicenseInfo;

[Serializable]
public sealed class AggregateFeedDictionary : Dictionary<string, ILicenseFeed>, IAggregateFeedDictionary, IDictionary<string, ILicenseFeed>, ICollection<KeyValuePair<string, ILicenseFeed>>, IEnumerable<KeyValuePair<string, ILicenseFeed>>, IEnumerable
{
	public int[] EnabledCameraIndexes { get; }

	public AggregateFeedDictionary()
	{
		EnabledCameraIndexes = new int[0];
	}

	public AggregateFeedDictionary(List<IHardwareInfo> hardwareInfos)
	{
		if (hardwareInfos == null)
		{
			throw new ArgumentNullException("hardwareInfos");
		}
		EnabledCameraIndexes = ProcessFeeds(hardwareInfos);
	}

	private AggregateFeedDictionary(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	private int[] ProcessFeeds(List<IHardwareInfo> hardwareInfos)
	{
		if (hardwareInfos.Count > 0)
		{
			if (hardwareInfos[0].IsInterconnectedProduct)
			{
				CalculateFeedCountsFromICPHardwareInfos(hardwareInfos);
				return new int[0];
			}
			return CalculateFeedCountsFromHardwareInfos(hardwareInfos);
		}
		return new int[0];
	}

	private int[] CalculateFeedCountsFromHardwareInfos(List<IHardwareInfo> hardwareInfos)
	{
		List<int> list = new List<int>();
		Dictionary<string, HashSet<int>> dictionary = new Dictionary<string, HashSet<int>>();
		string macAddress = null;
		foreach (IHardwareInfo hardwareInfo in hardwareInfos)
		{
			ValidateMacAddress(hardwareInfo.MacAddress, ref macAddress);
			ValidateIsNonIcp(hardwareInfo.IsInterconnectedProduct);
			foreach (IDeviceInfo enabledDevice in hardwareInfo.EnabledDevices)
			{
				if (!dictionary.ContainsKey(enabledDevice.DeviceType))
				{
					dictionary.Add(enabledDevice.DeviceType, new HashSet<int>());
				}
				if (dictionary[enabledDevice.DeviceType].Add(enabledDevice.DeviceIndex))
				{
					IncrementCount(enabledDevice.DeviceType);
					if (enabledDevice.DeviceType.Equals("Camera", StringComparison.OrdinalIgnoreCase))
					{
						list.Add(enabledDevice.DeviceIndex);
					}
				}
			}
		}
		list.Sort();
		return list.ToArray();
	}

	private void CalculateFeedCountsFromICPHardwareInfos(List<IHardwareInfo> hardwareInfos)
	{
		Dictionary<string, HashSet<string>> dictionary = new Dictionary<string, HashSet<string>>();
		string macAddress = null;
		foreach (IHardwareInfo hardwareInfo in hardwareInfos)
		{
			ValidateMacAddress(hardwareInfo.MacAddress, ref macAddress);
			ValidateIsIcp(hardwareInfo.IsInterconnectedProduct);
			foreach (IDeviceInfo enabledDevice in hardwareInfo.EnabledDevices)
			{
				if (!dictionary.ContainsKey(enabledDevice.DeviceType))
				{
					dictionary.Add(enabledDevice.DeviceType, new HashSet<string>());
				}
				if (dictionary[enabledDevice.DeviceType].Add(enabledDevice.UniqueIdentifier))
				{
					IncrementCount(enabledDevice.DeviceType);
				}
			}
		}
	}

	private void IncrementCount(string feedType)
	{
		if (!ContainsKey(feedType))
		{
			Add(feedType, new Feed(feedType, 1));
		}
		else
		{
			base[feedType].Count++;
		}
	}

	private static void ValidateMacAddress(string currentMacAddress, ref string macAddress)
	{
		if (currentMacAddress == null)
		{
			throw new ArgumentException("All hardware infos should have a non-null mac address");
		}
		if (macAddress == null)
		{
			macAddress = currentMacAddress;
		}
		else if (!currentMacAddress.Equals(macAddress, StringComparison.OrdinalIgnoreCase))
		{
			throw new ArgumentException("All hardware infos should have the same mac address");
		}
	}

	private static void ValidateIsIcp(bool isIcp)
	{
		if (!isIcp)
		{
			throw new ArgumentException("Cannot mix ICP and non-ICP hardware");
		}
	}

	private static void ValidateIsNonIcp(bool isIcp)
	{
		if (isIcp)
		{
			throw new ArgumentException("Cannot mix ICP and non-ICP hardware");
		}
	}
}
