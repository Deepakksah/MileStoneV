using System;

namespace VideoOS.Management.VmoClient;

public class ViewLayoutCollection : VmoObjectGuidBasedCollection<ViewLayout>
{
	public ViewLayoutCollection(LoadMethod loadViewLayouts, VmoObject parent, bool isInitialized)
		: base((Func<bool, ViewLayout[]>)loadViewLayouts.Invoke, parent, isInitialized)
	{
	}
}
