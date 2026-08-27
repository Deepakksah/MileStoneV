using System;
using System.Collections.Generic;

namespace VideoOS.Common.Integration.Configuration;

public class DeviceGroupItems
{
	private ConfigurationManager configurationManager;

	private RollbackDictionary<Guid, DeviceGroup> dictionary;

	private List<Guid> childGroupsLoaded;

	public DeviceGroupItems(ConfigurationManager configurationManager)
	{
		this.configurationManager = configurationManager;
		dictionary = new RollbackDictionary<Guid, DeviceGroup>(configurationManager, this);
		childGroupsLoaded = new List<Guid>();
		foreach (ConfigurationDataSet.DeviceGroupsRow row in this.configurationManager.Dataset.DeviceGroups.Rows)
		{
			AddItem(row);
		}
	}

	public DeviceGroup AddNew(Guid id, string name, string deviceType, DeviceGroup parent, string description, bool builtIn, bool autoInsertNew, bool isUserControlled)
	{
		string iDParentDeviceGroup = parent?.ID.ToString();
		ConfigurationDataSet.DeviceGroupsRow row = configurationManager.Dataset.DeviceGroups.AddDeviceGroupsRow(DateTime.UtcNow, id.ToString(), iDParentDeviceGroup, deviceType, name, description, builtIn, autoInsertNew, isUserControlled);
		return AddItem(row);
	}

	public void Remove(Guid id)
	{
		GetDeviceGroupByDeviceGroupId(id).DataRow.Delete();
		dictionary.Remove(id);
		childGroupsLoaded.Remove(id);
	}

	public DeviceGroup GetDeviceGroupByDeviceGroupId(Guid id)
	{
		if (dictionary.TryGetValue(id, out var value))
		{
			return value;
		}
		if (!configurationManager.UseDynamicLoad)
		{
			throw ConfigurationItemNotFoundException.CreateWithId(typeof(DeviceGroup), id);
		}
		using ConfigurationDataSet configurationDataSet = configurationManager.dataLoader.GetDeviceGroupByDeviceGroupId(id);
		if (configurationDataSet.DeviceGroups.Rows.Count == 0)
		{
			throw ConfigurationItemNotFoundException.CreateWithId(typeof(DeviceGroup), id);
		}
		configurationManager.DisableEventHandling = true;
		configurationManager.BeginLoadData(configurationManager.Dataset.DeviceGroups);
		ConfigurationDataSet.DeviceGroupsRow deviceGroupsRow = (ConfigurationDataSet.DeviceGroupsRow)configurationDataSet.DeviceGroups.Rows[0];
		configurationManager.Dataset.DeviceGroups.ImportRow(deviceGroupsRow);
		ConfigurationDataSet.DeviceGroupsRow row = configurationManager.Dataset.DeviceGroups.FindByIDDeviceGroup(deviceGroupsRow.IDDeviceGroup);
		value = AddItem(row);
		configurationManager.EndLoadData(configurationManager.Dataset.DeviceGroups);
		configurationManager.DisableEventHandling = false;
		return value;
	}

	public DeviceGroup[] GetChildGroups(Guid parentId)
	{
		List<DeviceGroup> list = new List<DeviceGroup>();
		if (!configurationManager.UseDynamicLoad || childGroupsLoaded.Contains(parentId))
		{
			foreach (DeviceGroup value2 in dictionary.Values)
			{
				_ = value2.Parent;
				if ((value2.Parent == null && parentId == Guid.Empty) || (value2.Parent != null && value2.Parent.ID == parentId))
				{
					list.Add(value2);
				}
			}
		}
		else
		{
			using ConfigurationDataSet configurationDataSet = configurationManager.dataLoader.GetDeviceGroupsByParentDeviceGroupId(parentId);
			configurationManager.DisableEventHandling = true;
			configurationManager.BeginLoadData(configurationManager.Dataset.DeviceGroups);
			foreach (ConfigurationDataSet.DeviceGroupsRow row2 in configurationDataSet.DeviceGroups.Rows)
			{
				Guid key = new Guid(row2.IDDeviceGroup);
				if (!dictionary.TryGetValue(key, out var value))
				{
					configurationManager.Dataset.DeviceGroups.ImportRow(row2);
					ConfigurationDataSet.DeviceGroupsRow row = configurationManager.Dataset.DeviceGroups.FindByIDDeviceGroup(row2.IDDeviceGroup);
					value = AddItem(row);
				}
				list.Add(value);
			}
			configurationManager.EndLoadData(configurationManager.Dataset.DeviceGroups);
			configurationManager.DisableEventHandling = false;
			childGroupsLoaded.Add(parentId);
		}
		return list.ToArray();
	}

	private DeviceGroup AddItem(ConfigurationDataSet.DeviceGroupsRow row)
	{
		DeviceGroup deviceGroup = new DeviceGroup(configurationManager, row);
		dictionary.Add(deviceGroup.ID, deviceGroup);
		return deviceGroup;
	}
}
