using System;

namespace VideoOS.Management.VmoClient;

public class ViewCollection : VmoObjectGuidBasedCollection<View>
{
	public ViewCollection(LoadMethod loadViews, VmoObject parent, bool isInitialized)
		: base((Func<bool, View[]>)loadViews.Invoke, parent, isInitialized)
	{
	}
}
