using System;

namespace VideoOS.Management.VmoClient;

public class OutputDeviceCollection : VmoObjectGuidBasedCollection<OutputDevice>
{
	public OutputDeviceCollection(LoadMethod loadMethod, VmoObject parent, bool isInitialized)
		: base((Func<bool, OutputDevice[]>)loadMethod.Invoke, parent, isInitialized)
	{
	}
}
