using System;
using System.Collections;
using System.Collections.Generic;

namespace VideoOS.Common.Integration.Configuration;

public class DeviceGroups : IEnumerable<DeviceGroup>, IEnumerable
{
	protected ConfigurationManager configurationManager;

	protected DeviceGroup parent;

	private RollbackDictionary<Guid, DeviceGroup> dictionary;

	public DeviceGroup this[Guid id] => dictionary[id];

	public DeviceGroups(ConfigurationManager configurationManager, DeviceGroup parent)
	{
		this.configurationManager = configurationManager;
		this.parent = parent;
		dictionary = new RollbackDictionary<Guid, DeviceGroup>(configurationManager, this);
		Guid parentId = parent?.ID ?? Guid.Empty;
		DeviceGroup[] childGroups = this.configurationManager.DeviceGroupItems.GetChildGroups(parentId);
		foreach (DeviceGroup deviceGroup in childGroups)
		{
			dictionary.Add(deviceGroup.ID, deviceGroup);
		}
	}

	public DeviceGroup AddNew(Guid id, string name, string deviceType, string description, bool builtIn, bool autoInsertNew, bool isUserControlled)
	{
		DeviceGroup deviceGroup = configurationManager.DeviceGroupItems.AddNew(id, name, deviceType, parent, description, builtIn, autoInsertNew, isUserControlled);
		dictionary.Add(deviceGroup.ID, deviceGroup);
		return deviceGroup;
	}

	public void Remove(DeviceGroup deviceGroup)
	{
		if (deviceGroup == null)
		{
			throw new ArgumentNullException("deviceGroup");
		}
		deviceGroup.Devices.Clear();
		DeviceGroup[] array = deviceGroup.DeviceGroups.ToArray();
		foreach (DeviceGroup deviceGroup2 in array)
		{
			deviceGroup.DeviceGroups.Remove(deviceGroup2);
		}
		dictionary.Remove(deviceGroup.ID);
		configurationManager.DeviceGroupItems.Remove(deviceGroup.ID);
	}

	public bool HasGroupWithName(string name)
	{
		foreach (DeviceGroup value in dictionary.Values)
		{
			if (string.Equals(value.Name, name, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}
		return false;
	}

	public IEnumerator<DeviceGroup> GetEnumerator()
	{
		return dictionary.Values.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return dictionary.Values.GetEnumerator();
	}

	public DeviceGroup[] ToArray()
	{
		DeviceGroup[] array = new DeviceGroup[dictionary.Count];
		dictionary.Values.CopyTo(array, 0);
		return array;
	}
}
