using System;

namespace VideoOS.Management.VmoClient;

public class FailoverServerCollection : VmoObjectGuidBasedCollection<FailoverServer>
{
	public FailoverServerCollection(LoadMethod loadViews, VmoObject parent, bool isInitialized)
		: base((Func<bool, FailoverServer[]>)loadViews.Invoke, parent, isInitialized)
	{
	}
}
