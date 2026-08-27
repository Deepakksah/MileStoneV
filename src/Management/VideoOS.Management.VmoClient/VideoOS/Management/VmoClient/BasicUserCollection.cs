using System;

namespace VideoOS.Management.VmoClient;

public class BasicUserCollection : VmoObjectGuidBasedCollection<BasicUser>
{
	public BasicUserCollection(LoadMethod loadMethod, VmoObject parent, bool isInitialized)
		: base((Func<bool, BasicUser[]>)loadMethod.Invoke, parent, isInitialized)
	{
	}
}
