using System;

namespace VideoOS.Management.VmoClient;

public class PublicViewGroupCollection : VmoObjectGuidBasedCollection<PublicViewGroup>
{
	public PublicViewGroupCollection(LoadMethod loadMethod, VmoObject parent, bool isInitialized)
		: base((Func<bool, PublicViewGroup[]>)loadMethod.Invoke, parent, isInitialized)
	{
	}
}
