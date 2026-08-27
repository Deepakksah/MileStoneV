using System;

namespace VideoOS.Management.VmoClient;

public class RoleCollection : VmoObjectGuidBasedCollection<Role>
{
	public RoleCollection(LoadMethod loadMethod, VmoObject parent, bool isInitialized)
		: base((Func<bool, Role[]>)loadMethod.Invoke, parent, isInitialized)
	{
	}
}
