using System;

namespace VideoOS.Management.VmoClient;

public class HardwareCollection : VmoObjectGuidBasedCollection<Hardware>
{
	public HardwareCollection(LoadMethod loadHardware, VmoObject parent, bool isInitialized)
		: base((Func<bool, Hardware[]>)loadHardware.Invoke, parent, isInitialized)
	{
	}
}
