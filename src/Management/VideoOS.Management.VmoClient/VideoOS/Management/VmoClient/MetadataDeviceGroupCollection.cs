using System;

namespace VideoOS.Management.VmoClient;

public class MetadataDeviceGroupCollection : VmoObjectGuidBasedCollection<MetadataDeviceGroup>
{
	public MetadataDeviceGroupCollection(LoadMethod loadDeviceGroups, VmoObject parent, bool isInitialized)
		: base((Func<bool, MetadataDeviceGroup[]>)loadDeviceGroups.Invoke, parent, isInitialized)
	{
	}
}
