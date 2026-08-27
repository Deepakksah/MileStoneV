using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Common.Integration.Configuration;

public class DeviceGroupMemberItems
{
	private ConfigurationManager configurationManager;

	private RollbackDictionary<DeviceGroupMemberKey, DeviceGroupMember> dictionary;

	private List<Guid> groupsLoaded;

	public DeviceGroupMemberItems(ConfigurationManager configurationManager)
	{
		this.configurationManager = configurationManager;
		dictionary = new RollbackDictionary<DeviceGroupMemberKey, DeviceGroupMember>(configurationManager, this);
		groupsLoaded = new List<Guid>();
		foreach (ConfigurationDataSet.DeviceGroupMembersRow row in this.configurationManager.Dataset.DeviceGroupMembers.Rows)
		{
			AddItem(row);
		}
	}

	public DeviceGroupMember AddNew(Guid deviceGroupId, Guid deviceId)
	{
		ConfigurationDataSet.DeviceGroupMembersRow row = configurationManager.Dataset.DeviceGroupMembers.AddDeviceGroupMembersRow(DateTime.UtcNow, deviceGroupId.ToString(), deviceId.ToString());
		return AddItem(row);
	}

	public void Remove(Guid deviceGroupId, Guid deviceId)
	{
		DeviceGroupMemberKey key = new DeviceGroupMemberKey(deviceGroupId, deviceId);
		if (!dictionary.TryGetValue(key, out var value))
		{
			GetDeviceGroupMembersByDeviceGroup(deviceGroupId);
			if (!dictionary.TryGetValue(key, out value))
			{
				return;
			}
		}
		value.DataRow.Delete();
		dictionary.Remove(key);
	}

	public void RemoveByDeviceGroupId(Guid deviceGroupId)
	{
		DeviceGroupMember[] deviceGroupMembersByDeviceGroup = GetDeviceGroupMembersByDeviceGroup(deviceGroupId);
		foreach (DeviceGroupMember deviceGroupMember in deviceGroupMembersByDeviceGroup)
		{
			Remove(deviceGroupMember.DeviceGroupId, deviceGroupMember.DeviceId);
		}
		groupsLoaded.Remove(deviceGroupId);
	}

	public DeviceGroupMember[] GetDeviceGroupMembersByDeviceGroup(Guid deviceGroupId)
	{
		List<DeviceGroupMember> list = new List<DeviceGroupMember>();
		if (!configurationManager.UseDynamicLoad || groupsLoaded.Contains(deviceGroupId))
		{
			foreach (DeviceGroupMember value in dictionary.Values)
			{
				if (value.DeviceGroupId == deviceGroupId)
				{
					list.Add(value);
				}
			}
		}
		else
		{
			using (ConfigurationDataSet dataset = configurationManager.dataLoader.GetDeviceGroupMembersByDeviceGroupId(deviceGroupId))
			{
				list = CreateItemListFromDataSet(dataset);
			}
			groupsLoaded.Add(deviceGroupId);
		}
		return list.ToArray();
	}

	public DeviceGroupMember[] GetDeviceGroupMembersByDevice(Guid deviceId)
	{
		List<DeviceGroupMember> list = new List<DeviceGroupMember>();
		if (!configurationManager.UseDynamicLoad)
		{
			foreach (DeviceGroupMember value in dictionary.Values)
			{
				if (value.DeviceId == deviceId)
				{
					list.Add(value);
				}
			}
		}
		else
		{
			using ConfigurationDataSet dataset = configurationManager.dataLoader.GetDeviceGroupMembersByDeviceId(deviceId);
			list = CreateItemListFromDataSet(dataset);
		}
		return list.ToArray();
	}

	private List<DeviceGroupMember> CreateItemListFromDataSet(ConfigurationDataSet dataset)
	{
		List<DeviceGroupMember> list = new List<DeviceGroupMember>();
		configurationManager.DisableEventHandling = true;
		configurationManager.BeginLoadData(configurationManager.Dataset.DeviceGroupMembers);
		foreach (ConfigurationDataSet.DeviceGroupMembersRow row2 in dataset.DeviceGroupMembers.Rows)
		{
			DeviceGroupMemberKey key = new DeviceGroupMemberKey(new Guid(row2.IDDeviceGroup), new Guid(row2.IDDevice));
			if (!dictionary.TryGetValue(key, out var value))
			{
				configurationManager.Dataset.DeviceGroupMembers.ImportRow(row2);
				ConfigurationDataSet.DeviceGroupMembersRow row = configurationManager.Dataset.DeviceGroupMembers.FindByIDDeviceGroupIDDevice(row2.IDDeviceGroup, row2.IDDevice);
				value = AddItem(row);
			}
			list.Add(value);
		}
		configurationManager.DisableEventHandling = false;
		configurationManager.EndLoadData(configurationManager.Dataset.DeviceGroupMembers);
		return list;
	}

	private DeviceGroupMember AddItem(ConfigurationDataSet.DeviceGroupMembersRow row)
	{
		DeviceGroupMember deviceGroupMember = new DeviceGroupMember(configurationManager, row);
		if (!dictionary.ContainsKey(new DeviceGroupMemberKey(deviceGroupMember.DeviceGroupId, deviceGroupMember.DeviceId)))
		{
			dictionary.Add(new DeviceGroupMemberKey(deviceGroupMember.DeviceGroupId, deviceGroupMember.DeviceId), deviceGroupMember);
		}
		else
		{
			LoggerExtensions.LogWarning(Telemetry.GetILogger("MainApplication"), "[Duplicate DeviceGroupMember]: DeviceId: {DeviceId}, DeviceGroupId: {DeviceGroupId}", new object[2] { deviceGroupMember.DeviceId, deviceGroupMember.DeviceGroupId });
		}
		return deviceGroupMember;
	}
}
