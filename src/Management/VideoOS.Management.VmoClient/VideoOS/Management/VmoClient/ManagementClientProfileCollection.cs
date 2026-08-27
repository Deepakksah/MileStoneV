using System;

namespace VideoOS.Management.VmoClient;

public class ManagementClientProfileCollection : VmoObjectGuidBasedCollection<ManagementClientProfile>
{
	public ManagementClientProfileCollection(LoadMethod loadMethod, VmoObject parent, bool isInitialized)
		: base((Func<bool, ManagementClientProfile[]>)loadMethod.Invoke, parent, isInitialized)
	{
	}
}
