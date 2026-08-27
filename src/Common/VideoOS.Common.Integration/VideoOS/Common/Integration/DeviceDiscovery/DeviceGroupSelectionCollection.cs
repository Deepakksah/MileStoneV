using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace VideoOS.Common.Integration.DeviceDiscovery;

public class DeviceGroupSelectionCollection : Collection<DeviceGroupSelection>
{
	public void Add(string type, string name, Guid id)
	{
		Add(new DeviceGroupSelection(type, name, id));
	}

	public bool Contains(string deviceType)
	{
		return this.Any((DeviceGroupSelection t) => t.DeviceType == deviceType);
	}

	public void Update(string deviceType, Guid id)
	{
		DeviceGroupSelection deviceGroupSelection = this.FirstOrDefault((DeviceGroupSelection t) => t.DeviceType == deviceType);
		if (deviceGroupSelection != null)
		{
			deviceGroupSelection.IDDeviceGroup = id;
		}
	}
}
