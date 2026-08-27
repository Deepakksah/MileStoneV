using System;

namespace VideoOS.Management.VmoClient;

public class MIPActionCollection : VmoObjectGuidBasedCollection<MIPAction>
{
	public MIPActionCollection(LoadMethod loadViews, VmoObject parent, bool isInitialized)
		: base((Func<bool, MIPAction[]>)loadViews.Invoke, parent, isInitialized)
	{
	}
}
