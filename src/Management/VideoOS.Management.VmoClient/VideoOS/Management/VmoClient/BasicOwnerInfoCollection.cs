using System;

namespace VideoOS.Management.VmoClient;

public sealed class BasicOwnerInfoCollection : VmoObjectGuidBasedCollection<BasicOwnerInfo>
{
	public BasicOwnerInfoCollection(LoadMethod loadMethod, VmoObject parent, bool isInitialized)
		: base((Func<bool, BasicOwnerInfo[]>)loadMethod.Invoke, parent, isInitialized)
	{
	}
}
