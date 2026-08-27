using System;

namespace VideoOS.Management.VmoClient;

public class DeviceCollection : VmoObjectGuidBasedCollection<Device>
{
	public DeviceCollection(LoadMethod loadMethod, VmoObject parent, bool isInitialized)
		: base((Func<bool, Device[]>)loadMethod.Invoke, parent, isInitialized)
	{
	}
}
