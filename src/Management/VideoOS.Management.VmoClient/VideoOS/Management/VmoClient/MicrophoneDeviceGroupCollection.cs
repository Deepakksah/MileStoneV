using System;

namespace VideoOS.Management.VmoClient;

public class MicrophoneDeviceGroupCollection : VmoObjectGuidBasedCollection<MicrophoneDeviceGroup>
{
	public MicrophoneDeviceGroupCollection(LoadMethod loadDeviceGroups, VmoObject parent, bool isInitialized)
		: base((Func<bool, MicrophoneDeviceGroup[]>)loadDeviceGroups.Invoke, parent, isInitialized)
	{
	}
}
