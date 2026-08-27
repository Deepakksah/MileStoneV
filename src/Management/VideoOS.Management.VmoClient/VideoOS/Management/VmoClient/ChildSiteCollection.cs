using System;

namespace VideoOS.Management.VmoClient;

public class ChildSiteCollection : VmoObjectGuidBasedCollection<ChildSite>
{
	public ChildSiteCollection(LoadMethod loadMethod, VmoObject parent, bool isInitialized)
		: base((Func<bool, ChildSite[]>)loadMethod.Invoke, parent, isInitialized)
	{
	}
}
