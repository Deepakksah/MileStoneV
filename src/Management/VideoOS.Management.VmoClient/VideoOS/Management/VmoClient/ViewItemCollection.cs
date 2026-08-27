using System;

namespace VideoOS.Management.VmoClient;

public class ViewItemCollection : VmoObjectGuidBasedCollection<ViewItem>
{
	public ViewItemCollection(LoadMethod loadViews, VmoObject parent, bool isInitialized)
		: base((Func<bool, ViewItem[]>)loadViews.Invoke, parent, isInitialized)
	{
	}
}
