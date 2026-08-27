using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server.ConfigurationApi;

public class DeviceGroupCache
{
	public class ItemTypeCache
	{
		public Dictionary<Guid, HashSet<Guid>> GroupMembers = new Dictionary<Guid, HashSet<Guid>>();

		public List<Tuple<Guid, Guid>> GroupGroups = new List<Tuple<Guid, Guid>>();

		public Dictionary<Guid, Device> Devices = new Dictionary<Guid, Device>();

		public Dictionary<Guid, DeviceGroup> DeviceGroups = new Dictionary<Guid, DeviceGroup>();

		public bool DevicesLoaded;

		public bool GroupsLoaded;

		public string ItemType { get; set; }
	}

	private Dictionary<string, ItemTypeCache> _cache = new Dictionary<string, ItemTypeCache>();

	private bool _prepareDone;

	private bool _isAdministrator;

	public void Prepare(IGeminiRequestContext context, bool isAdministrator, HashSet<string> itemTypes, bool includeMembers)
	{
		if (!_prepareDone)
		{
			_isAdministrator = isAdministrator;
			LoadGroupsAndMembers(context, itemTypes, includeMembers);
			_prepareDone = true;
		}
	}

	private void LoadGroupsAndMembers(IGeminiRequestContext requestContext, HashSet<string> itemTypes, bool includeMembers)
	{
		foreach (string itemType in itemTypes)
		{
			if (!IsDeviceGroup(itemType) || _cache.ContainsKey(itemType))
			{
				continue;
			}
			DeviceType deviceType = DeviceGroupConfigurationApi.ConvertItemTypeToDeviceType(itemType);
			Dictionary<Guid, HashSet<Guid>> groupMembers = null;
			Dictionary<Guid, DeviceGroup> dictionary = new Dictionary<Guid, DeviceGroup>();
			bool groupsLoaded = false;
			List<Tuple<Guid, Guid>> list;
			if (_isAdministrator)
			{
				list = new List<Tuple<Guid, Guid>>();
				DeviceGroup[] array = DeviceGroup.QueryDeviceGroupsNoSecurity(requestContext, deviceType);
				foreach (DeviceGroup deviceGroup in array)
				{
					Guid guid = new Guid(deviceGroup.Id);
					Guid item = (string.IsNullOrEmpty(deviceGroup.ParentGroupId) ? Guid.Empty : new Guid(deviceGroup.ParentGroupId));
					dictionary.Add(guid, deviceGroup);
					list.Add(new Tuple<Guid, Guid>(item, guid));
				}
				groupsLoaded = true;
				if (includeMembers)
				{
					groupMembers = DeviceGroupData.QueryDeviceGroupMembersForType(requestContext, deviceType);
				}
			}
			else
			{
				list = DeviceGroup.QueryDeviceGroupParentAndGroupIdsNonAdmin(requestContext, null, deviceType);
				if (includeMembers)
				{
					groupMembers = DeviceGroupData.QueryDeviceGroupMembersNonAdmin(requestContext, deviceType);
				}
			}
			ItemTypeCache value = new ItemTypeCache
			{
				ItemType = itemType,
				GroupGroups = list,
				GroupMembers = groupMembers,
				DeviceGroups = dictionary,
				GroupsLoaded = groupsLoaded
			};
			_cache.Add(itemType, value);
		}
	}

	public IEnumerable<Guid> QuerySubGroups(string groupItemType, Guid parentGroupId)
	{
		if (_cache.TryGetValue(groupItemType, out var value))
		{
			List<Guid> list = new List<Guid>();
			{
				foreach (Tuple<Guid, Guid> groupGroup in value.GroupGroups)
				{
					if (groupGroup.Item1 == parentGroupId)
					{
						list.Add(groupGroup.Item2);
					}
				}
				return list;
			}
		}
		throw new DeviceGroupNotFoundException();
	}

	public IEnumerable<Guid> QueryDevicesInGroup(string groupItemType, Guid groupId)
	{
		if (_cache.TryGetValue(groupItemType, out var value) && value.GroupMembers.TryGetValue(groupId, out var value2))
		{
			return value2;
		}
		return new HashSet<Guid>();
	}

	public void LoadDevicesForGroup(IGeminiRequestContext requestContext, string itemType, IEnumerable<Guid> deviceIds)
	{
		string key = itemType + "Group";
		if (!_cache.TryGetValue(key, out var value))
		{
			return;
		}
		List<string> list = new List<string>();
		foreach (Guid deviceId in deviceIds)
		{
			if (!value.Devices.ContainsKey(deviceId))
			{
				list.Add(deviceId.ToString());
			}
		}
		if (list.Any())
		{
			string[] ids = list.ToArray();
			Device[] array = Device.QueryDevicesByIds(requestContext, ids);
			foreach (Device device in array)
			{
				value.Devices[new Guid(device.Id)] = device;
			}
		}
	}

	public void LoadDevicesForHierarchy(IGeminiRequestContext requestContext, string itemType)
	{
		string key = itemType + "Group";
		if (_cache.TryGetValue(key, out var value) && !value.DevicesLoaded)
		{
			DeviceTypeSearch deviceTypeSearch = DeviceConfigurationApi.ConvertItemTypeToDeviceTypeSearch(itemType);
			Device[] array = Device.QueryDevicesByTypeAndFilter(requestContext, deviceTypeSearch, string.Empty);
			foreach (Device device in array)
			{
				value.Devices[new Guid(device.Id)] = device;
			}
			value.DevicesLoaded = true;
		}
	}

	public void LoadGroupsForHierarchy(IGeminiRequestContext requestContext, string groupItemType)
	{
		if (!_cache.TryGetValue(groupItemType, out var value) || value.GroupsLoaded)
		{
			return;
		}
		HashSet<Guid> hashSet = new HashSet<Guid>();
		foreach (Tuple<Guid, Guid> groupGroup in value.GroupGroups)
		{
			if (groupGroup.Item1 != Guid.Empty)
			{
				hashSet.Add(groupGroup.Item1);
			}
			hashSet.Add(groupGroup.Item2);
		}
		DeviceGroup[] array = DeviceGroup.QueryDeviceGroupsNoSecurity(requestContext, hashSet.Select((Guid g) => g.ToString()).ToArray());
		foreach (DeviceGroup deviceGroup in array)
		{
			value.DeviceGroups[new Guid(deviceGroup.Id)] = deviceGroup;
		}
		value.GroupsLoaded = true;
	}

	public void LoadGroupsForList(IGeminiRequestContext requestContext, string groupItemType, IEnumerable<Guid> groupIds)
	{
		if (!_cache.TryGetValue(groupItemType, out var value))
		{
			return;
		}
		List<string> list = new List<string>();
		foreach (Guid groupId in groupIds)
		{
			if (!value.DeviceGroups.ContainsKey(groupId))
			{
				list.Add(groupId.ToString());
			}
		}
		DeviceGroup[] array = DeviceGroup.QueryDeviceGroupsNoSecurity(requestContext, list.ToArray());
		foreach (DeviceGroup deviceGroup in array)
		{
			value.DeviceGroups[new Guid(deviceGroup.Id)] = deviceGroup;
		}
	}

	public IEnumerable<Device> LoadDevicesFromCache(string itemType, IEnumerable<Guid> deviceIds)
	{
		string key = itemType + "Group";
		List<Device> list = new List<Device>();
		if (_cache.TryGetValue(key, out var value))
		{
			foreach (Guid deviceId in deviceIds)
			{
				if (value.Devices.TryGetValue(deviceId, out var value2))
				{
					list.Add(value2);
				}
			}
		}
		return list;
	}

	public IEnumerable<DeviceGroup> LoadDeviceGroupsFromCache(string groupItemType, IEnumerable<Guid> deviceGroupIds)
	{
		List<DeviceGroup> list = new List<DeviceGroup>();
		if (_cache.TryGetValue(groupItemType, out var value))
		{
			foreach (Guid deviceGroupId in deviceGroupIds)
			{
				if (value.DeviceGroups.TryGetValue(deviceGroupId, out var value2))
				{
					list.Add(value2);
				}
			}
		}
		return list;
	}

	private static bool IsDeviceGroup(string itemType)
	{
		switch (itemType)
		{
		default:
			return itemType == "OutputGroup";
		case "CameraGroup":
		case "MicrophoneGroup":
		case "SpeakerGroup":
		case "MetadataGroup":
		case "InputEventGroup":
			return true;
		}
	}
}
