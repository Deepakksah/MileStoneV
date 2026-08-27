using System;

namespace VideoOS.Management.VmoClient;

public class OutputDeviceGroupCollection : VmoObjectGuidBasedCollection<OutputDeviceGroup>
{
	public OutputDeviceGroupCollection(LoadMethod loadDeviceGroups, VmoObject parent, bool isInitialized)
		: base((Func<bool, OutputDeviceGroup[]>)loadDeviceGroups.Invoke, parent, isInitialized)
	{
	}
}
