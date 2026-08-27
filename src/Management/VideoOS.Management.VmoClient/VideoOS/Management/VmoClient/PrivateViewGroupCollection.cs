using System;

namespace VideoOS.Management.VmoClient;

public class PrivateViewGroupCollection : VmoObjectGuidBasedCollection<PrivateViewGroup>
{
	public PrivateViewGroupCollection(LoadMethod loadMethod, VmoObject parent, bool isInitialized)
		: base((Func<bool, PrivateViewGroup[]>)loadMethod.Invoke, parent, isInitialized)
	{
	}
}
