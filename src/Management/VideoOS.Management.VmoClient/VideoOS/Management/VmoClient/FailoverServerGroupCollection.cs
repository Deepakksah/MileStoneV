using System;

namespace VideoOS.Management.VmoClient;

public class FailoverServerGroupCollection : VmoObjectGuidBasedCollection<FailoverServerGroup>
{
	public FailoverServerGroupCollection(LoadMethod loadMethod, VmoObject parent, bool isInitialized)
		: base((Func<bool, FailoverServerGroup[]>)loadMethod.Invoke, parent, isInitialized)
	{
	}
}
