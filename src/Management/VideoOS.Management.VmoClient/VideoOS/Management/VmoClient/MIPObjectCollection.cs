using System;

namespace VideoOS.Management.VmoClient;

public class MIPObjectCollection : VmoObjectGuidBasedCollection<MIPObject>
{
	public MIPObjectCollection(LoadMethod loadViews, VmoObject parent, bool isInitialized)
		: base((Func<bool, MIPObject[]>)loadViews.Invoke, parent, isInitialized)
	{
	}
}
