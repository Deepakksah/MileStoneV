using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Common.Integration.Configuration;

public class DeviceGroupMembers : IEnumerable<Device>, IEnumerable
{
	private ConfigurationManager configurationManager;

	private DeviceGroup deviceGroup;

	private bool hardwareLoaded;

	private RollbackDictionary<DeviceGroupMemberKey, DeviceGroupMember> dictionary;

	public int Count => dictionary.Count;

	public DeviceGroupMembers(ConfigurationManager configurationManager, DeviceGroup deviceGroup)
	{
		this.configurationManager = configurationManager;
		this.deviceGroup = deviceGroup;
		dictionary = new RollbackDictionary<DeviceGroupMemberKey, DeviceGroupMember>(configurationManager, this);
		DeviceGroupMember[] deviceGroupMembersByDeviceGroup = this.configurationManager.DeviceGroupMemberItems.GetDeviceGroupMembersByDeviceGroup(this.deviceGroup.ID);
		foreach (DeviceGroupMember deviceGroupMember in deviceGroupMembersByDeviceGroup)
		{
			dictionary.Add(deviceGroupMember.Key, deviceGroupMember);
		}
	}

	public void Add(Device device)
	{
		if (device == null)
		{
			throw new ArgumentNullException("device");
		}
		DeviceGroupMember deviceGroupMember = configurationManager.DeviceGroupMemberItems.AddNew(deviceGroup.ID, device.ID);
		dictionary.Add(deviceGroupMember.Key, deviceGroupMember);
	}

	public void Remove(Device device)
	{
		if (device == null)
		{
			throw new ArgumentNullException("device");
		}
		configurationManager.DeviceGroupMemberItems.Remove(deviceGroup.ID, device.ID);
		dictionary.Remove(new DeviceGroupMemberKey(deviceGroup.ID, device.ID));
	}

	public void Clear()
	{
		configurationManager.DeviceGroupMemberItems.RemoveByDeviceGroupId(deviceGroup.ID);
		dictionary.Clear();
	}

	public bool Contains(Device device)
	{
		if (device == null)
		{
			throw new ArgumentNullException("device");
		}
		return dictionary.ContainsKey(new DeviceGroupMemberKey(deviceGroup.ID, device.ID));
	}

	public IEnumerator<Device> GetEnumerator()
	{
		Guid[] deviceIds = dictionary.Values.Select((DeviceGroupMember dgm) => dgm.DeviceId).ToArray();
		List<Device> list = new List<Device>(configurationManager.DeviceItems.GetDevicesByDeviceIds(deviceIds, supressItemNotFoundError: false));
		if (!hardwareLoaded)
		{
			HashSet<Guid> source = new HashSet<Guid>(list.Select((Device d) => d.IPHardwareID));
			configurationManager.HardwareItems.EnsureHardwareItemsLoaded(source.ToArray());
			hardwareLoaded = true;
		}
		return list.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
