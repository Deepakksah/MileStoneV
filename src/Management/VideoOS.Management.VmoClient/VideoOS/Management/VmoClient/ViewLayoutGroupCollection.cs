using System;

namespace VideoOS.Management.VmoClient;

public class ViewLayoutGroupCollection : VmoObjectGuidBasedCollection<ViewLayoutGroup>
{
	public ViewLayoutGroupCollection(LoadMethod loadMethod, VmoObject parent, bool isInitialized)
		: base((Func<bool, ViewLayoutGroup[]>)loadMethod.Invoke, parent, isInitialized)
	{
	}
}
