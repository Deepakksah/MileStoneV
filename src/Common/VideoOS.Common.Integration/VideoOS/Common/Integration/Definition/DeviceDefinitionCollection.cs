using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VideoOS.Common.Integration.Definition;

public class DeviceDefinitionCollection : KeyedCollection<DeviceHwid, DeviceDefinition>
{
	protected override DeviceHwid GetKeyForItem(DeviceDefinition item)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		return item.Hwid;
	}

	public bool TryGetValue(DeviceHwid hwid, out DeviceDefinition deviceDefinition)
	{
		deviceDefinition = null;
		try
		{
			deviceDefinition = base[hwid];
			return true;
		}
		catch (KeyNotFoundException)
		{
			return false;
		}
	}
}
