using System;

namespace VideoOS.Management.VmoClient;

public class SpeakerDeviceGroupCollection : VmoObjectGuidBasedCollection<SpeakerDeviceGroup>
{
	public SpeakerDeviceGroupCollection(LoadMethod loadDeviceGroups, VmoObject parent, bool isInitialized)
		: base((Func<bool, SpeakerDeviceGroup[]>)loadDeviceGroups.Invoke, parent, isInitialized)
	{
	}
}
