using System;

namespace VideoOS.Management.VmoClient;

public class InputDeviceCollection : VmoObjectGuidBasedCollection<InputDevice>
{
	public InputDeviceCollection(LoadMethod loadMethod, VmoObject parent, bool isInitialized)
		: base((Func<bool, InputDevice[]>)loadMethod.Invoke, parent, isInitialized)
	{
	}
}
