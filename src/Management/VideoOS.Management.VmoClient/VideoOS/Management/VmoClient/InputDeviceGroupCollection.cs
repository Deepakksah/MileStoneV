using System;

namespace VideoOS.Management.VmoClient;

public class InputDeviceGroupCollection : VmoObjectGuidBasedCollection<InputDeviceGroup>
{
	public InputDeviceGroupCollection(LoadMethod loadDeviceGroups, VmoObject parent, bool isInitialized)
		: base((Func<bool, InputDeviceGroup[]>)loadDeviceGroups.Invoke, parent, isInitialized)
	{
	}
}
