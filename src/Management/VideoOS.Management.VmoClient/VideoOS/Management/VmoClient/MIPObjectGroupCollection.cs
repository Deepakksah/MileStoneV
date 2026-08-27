using System;

namespace VideoOS.Management.VmoClient;

public class MIPObjectGroupCollection : VmoObjectGuidBasedCollection<MIPObjectGroup>
{
	public MIPObjectGroupCollection(LoadMethod loadMethod, VmoObject parent, bool isInitialized)
		: base((Func<bool, MIPObjectGroup[]>)loadMethod.Invoke, parent, isInitialized)
	{
	}
}
