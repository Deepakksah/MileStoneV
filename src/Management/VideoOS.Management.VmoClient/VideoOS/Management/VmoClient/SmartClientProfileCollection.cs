using System;

namespace VideoOS.Management.VmoClient;

public class SmartClientProfileCollection : VmoObjectGuidBasedCollection<SmartClientProfile>
{
	public SmartClientProfileCollection(LoadMethod loadMethod, VmoObject parent, bool isInitialized)
		: base((Func<bool, SmartClientProfile[]>)loadMethod.Invoke, parent, isInitialized)
	{
	}
}
